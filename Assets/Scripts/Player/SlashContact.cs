using PathCreation;
using UnityEngine;
using UnityEngine.Events;

public class SlashContact : MonoBehaviour {
    // Action to perform
    SlideAction slideAction;

    // For resets
    JumpAction jumpAction;
    DashAction dashAction;

    SlidableEnemy slidableEnemy;

    // Other variables
    float bloodGainAmount;
    UnityEvent<Collider> contactEvent;

    private void Start() {
        slideAction = GetComponentInParent<SlideAction>();
        jumpAction = GetComponentInParent<JumpAction>();
        dashAction = GetComponentInParent<DashAction>();

        // End of events
        slideAction.OnEndAction.AddListener(EndOfSlide);
    }

    private void StabContactEffect(Collider other) {
        bool found = false;
        bool canGainBlood = false;

        Vector3 slashDirection = transform.forward;

        if (other.TryGetComponent(out Slashable slashable)) {
            slashable.TriggerEffect();
            found = true;
        }
        
        if(other.TryGetComponent(out SlidableEnemy enemy)) {
            slidableEnemy = enemy;
            StartSlideAction(enemy.pathCreator, other, enemy.GetBonus());
            found = true;
        }
        else if (other.TryGetComponent(out PathCreator pathCreator)) {
            if (other.TryGetComponent(out WallDirection wallDir)) {
                Vector3 wallForward = wallDir.GetForwardVector();

                //Dot gives a value comparing the two directions, 1 = same direction, -1 = opposite direction
                float directionCheck = Vector3.Dot(slashDirection.normalized, wallForward.normalized);

                if (directionCheck < 0) {
                    StartSlideAction(pathCreator, other);
                    found = true;
                }
            }
            else {
                StartSlideAction(pathCreator, other);
                found = true;
            }
        }
        if (other.TryGetComponent(out SlashableEnviornment slashableEnviornment)) {
            canGainBlood = slashableEnviornment.canGiveBlood;
        }
        if(found) {
            EndContactEvent();
        }
        if(canGainBlood) {
            GetComponentInParent<BloodThirst>().GainBlood(bloodGainAmount, true);
        }
    }

    private void StartSlideAction(PathCreator pc, Collider other, float enemyBonus = 0) {
        GetComponent<PlayerActionManager>().ChangeAction(slideAction);
        slideAction.OnEndAction.AddListener(EndEnemy);
        slideAction.StartSlide(pc, other, enemyBonus);
    }

    private void EndEnemy() {
        if (slidableEnemy != null) {
            slidableEnemy.Die();
        }
        slideAction.OnEndAction.RemoveListener(EndEnemy);
    }

    public void ActivateContactEvent(UnityEvent<Collider> _contactEvent, float bloodGained) {
        bloodGainAmount = bloodGained; 
        contactEvent = _contactEvent;
        contactEvent.AddListener(StabContactEffect);
    }

    public void EndContactEvent() {
        contactEvent.RemoveListener(StabContactEffect);
    }

    private void EndOfSlide() {
        jumpAction.GiveAirJump();
        dashAction.ResetDash();
    }
}
