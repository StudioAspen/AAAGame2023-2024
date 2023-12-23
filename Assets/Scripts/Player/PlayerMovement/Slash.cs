using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;
using UnityEngine.UIElements;

public class Slash : MonoBehaviour
{
    [Header("Movement Variables")]
    public float slideSpeed;
    public float jumpMultiplier;

    [Header("Boosted Movement")]
    public float boostedSlideSpeed;
    public float boostedJumpMultiplier;

    [Header("Other Variables")]
    public float bloodGained;

    [Header("References")]
    [SerializeField] GameObject swordObject;
    [SerializeField] SwordAnimation swordAnimation;
    [SerializeField] WallSlideAction wallSlideAction;
    [SerializeField] MovementModification movementModification;
    [SerializeField] BloodThirst bloodThirst;
    [SerializeField] PlayerMovementStateManager playerMovementStateManager;

    // Components
    PlayerMovement playerMovement;
    DashMovement dashMovement;

    private void Start()
    {
        // Getting Components
        movementModification = GetComponent<MovementModification>();
        playerMovement = GetComponent<PlayerMovement>();
        dashMovement = GetComponent<DashMovement>();

        swordAnimation.OnContact.AddListener(SlashContact);
        swordAnimation.OnEndAnimation.AddListener(EndOfSlashAnimation);
    }

    public void StartSlash() {
        if (playerMovementStateManager.currentState == PlayerMovementState.IDLE) {
            //Animation Stuff (to be implemented later)
            playerMovementStateManager.ChangeState(PlayerMovementState.SLASHING);

            // Demon sword variables
            swordAnimation.StartSlashAnimation();
        }
    }

    public void InterruptSlash() {
        if (playerMovementStateManager.currentState == PlayerMovementState.SLASHING) {
            playerMovementStateManager.ChangeState(PlayerMovementState.IDLE);
            swordAnimation.EndAnimation();
        }
    }
    public void SlashContact(GameObject other) {
        if(playerMovementStateManager.currentState == PlayerMovementState.SLASHING) {
            SlashContactEffect(other, InterruptSlash);
        }
    }
    public bool SlashContactEffect(GameObject other, UnityAction interruptAction = null) {
        bool found = false;

        // Wall sliding when striking terrain with appropriate components
        if (other.TryGetComponent(out SlashableTerrain slashableTerrain) &&
            other.TryGetComponent(out PathCreator pc)) {
            interruptAction.Invoke();
            playerMovementStateManager.ChangeState(PlayerMovementState.WALL_SLIDING);
            wallSlideAction.OnSlideEnd.AddListener(EndOfWallSlide);

            float slideSpeedCalc = Mathf.Lerp(slideSpeed, boostedSlideSpeed, movementModification.boostForAll);
            wallSlideAction.StartSlide(pc, other, swordObject, slideSpeedCalc);

            found = true;
        }

        // Triggering generic effects
        if (other.TryGetComponent(out SlashedEffect slashedEffect)) {
            interruptAction.Invoke();

            slashedEffect.TriggerEffect();

            found = true;
        }

        if(found) {
            bloodThirst.GainBlood(bloodGained, true);
        }

        return found;
    }

    private void EndOfWallSlide() {
        wallSlideAction.OnSlideEnd.RemoveListener(EndOfWallSlide);
        playerMovementStateManager.ChangeState(PlayerMovementState.IDLE);

        playerMovement.Jump(Mathf.Lerp(jumpMultiplier, boostedJumpMultiplier, movementModification.boostForAll));

        playerMovement.ResetJump();
        dashMovement.ResetDash();
    }
    
    private void EndOfSlashAnimation() {
        if(playerMovementStateManager.currentState == PlayerMovementState.SLASHING) {
            playerMovementStateManager.ChangeState(PlayerMovementState.IDLE);
        }
    }

}
