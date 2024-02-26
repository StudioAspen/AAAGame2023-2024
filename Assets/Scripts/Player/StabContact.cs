using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabContact : MonoBehaviour
{
    // Contact Action
    DashThroughAction dashThroughAction;

    // For resets
    BasicMovementAction movementAction;
    DashAction dashAction;

    // Other variables
    UnityEvent OnContact = new UnityEvent();
    float bloodGainAmount;
    UnityEvent<Collider> contactEvent;
    private void Start() {
        dashThroughAction = transform.parent.GetComponent<DashThroughAction>();
        movementAction = transform.parent.GetComponent<BasicMovementAction>();
        dashAction = transform.parent.GetComponent<DashAction>();

        // End of events
        dashThroughAction.OnEndAction.AddListener(EndOfDash);
    }

    private void StabContactEffect(Collider other) {
        PlayerActionManager actionManager = GetComponent<PlayerActionManager>();
        if (other.gameObject.TryGetComponent(out Stabable stabable)) {
            stabable.TriggerEffect();
        }
        if (other.gameObject.TryGetComponent(out StabableEnviornment enviornment)) {
            if (enviornment.canGiveBlood) {
                GetComponent<BloodThirst>().GainBlood(bloodGainAmount, true);
            }

            // Setting up and starting dash
            actionManager.ChangeAction(dashThroughAction);
            dashThroughAction.DashThrough(enviornment);
        }
        EndContactEvent();
    }

    public void ActivateContactEvent(UnityEvent<Collider> _contactEvent, float bloodGained) {
        bloodGainAmount = bloodGained;
        contactEvent = _contactEvent;
        contactEvent.AddListener(StabContactEffect);
    }

    public void EndContactEvent() {
        OnContact.RemoveAllListeners();
        contactEvent.RemoveListener(StabContactEffect);
    }

    private void EndOfDash() {
        movementAction.ResetJump();
        dashAction.ResetDash();
    }
}
