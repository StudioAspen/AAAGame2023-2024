using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SlashContact : MonoBehaviour {
    // Action to perform
    SlideAction slideAction;

    // For resets
    BasicMovementAction movementAction;
    DashAction dashAction;

    // Other variables
    UnityEvent OnContact = new UnityEvent();
    float bloodGainAmount;
    UnityEvent<Collider> contactEvent;
    private void Start() {
        slideAction = transform.parent.GetComponent<SlideAction>();
        movementAction = transform.parent.GetComponent<BasicMovementAction>();
        dashAction = transform.parent.GetComponent<DashAction>();

        // End of events
        slideAction.OnEndAction.AddListener(EndOfSlide);
    }

    private void StabContactEffect(Collider other) {
        Vector3 slashDirection = transform.forward;

        if (other.TryGetComponent(out Slashable slashable)) {
            OnContact.Invoke();
            slashable.TriggerEffect();
        }

        if (other.TryGetComponent(out PathCreator pathCreator)) {
            if (other.TryGetComponent(out WallDirection wallDir)) {
                Vector3 wallForward = wallDir.GetForwardVector();

                //Dot gives a value comparing the two directions, 1 = same direction, -1 = opposite direction
                float directionCheck = Vector3.Dot(slashDirection.normalized, wallForward.normalized);

                if (directionCheck < 0) {
                    StartSlideAction(pathCreator, other);
                }
            }
            else {
                StartSlideAction(pathCreator, other);
            }
            OnContact.Invoke();
        }
        if (other.TryGetComponent(out SlashableEnviornment slashableEnviornment)) {
            if (slashableEnviornment.canGiveBlood) {
                GetComponent<BloodThirst>().GainBlood(bloodGainAmount, true);
            }
        }

        EndContactEvent();
    }

    private void StartSlideAction(PathCreator pc, Collider other) {
        GetComponent<PlayerActionManager>().ChangeAction(slideAction);
        slideAction.StartSlide(pc, other);
    }

    public void ActivateContactEvent(UnityEvent<Collider> _contactEvent, UnityAction contactCall, float bloodGained) {
        bloodGainAmount = bloodGained; 
        contactEvent = _contactEvent;
        contactEvent.AddListener(StabContactEffect);
        OnContact.AddListener(contactCall);
    }

    public void EndContactEvent() {
        OnContact.RemoveAllListeners();
        contactEvent.RemoveListener(StabContactEffect);
    }

    private void EndOfSlide() {
        movementAction.ResetJump();
        dashAction.ResetDash();
    }
}
