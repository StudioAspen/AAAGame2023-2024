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
        bool found = false;
        bool canGiveBlood = false;

        PlayerActionManager actionManager = GetComponent<PlayerActionManager>();
        if (other.gameObject.TryGetComponent(out Stabable stabable)) {
            found = true;
            stabable.TriggerEffect();
        }
        if (other.gameObject.TryGetComponent(out StabableEnviornment enviornment)) {
            canGiveBlood = enviornment.canGiveBlood;

            // Setting up and starting dash
            actionManager.ChangeAction(dashThroughAction);
            dashThroughAction.DashThrough(enviornment);

            found = true;
        }
        if(found) {
            EndContactEvent();
        }
        if(canGiveBlood) {
            GetComponentInParent<BloodThirst>().GainBlood(bloodGainAmount, true);
        }
    }

    public void ActivateContactEvent(UnityEvent<Collider> _contactEvent, float bloodGained) {
        bloodGainAmount = bloodGained;
        contactEvent = _contactEvent;
        contactEvent.AddListener(StabContactEffect);
    }

    public void EndContactEvent() {
        contactEvent.RemoveListener(StabContactEffect);
    }

    private void EndOfDash() {
        movementAction.GiveAirJump();
        dashAction.ResetDash();
    }
}
