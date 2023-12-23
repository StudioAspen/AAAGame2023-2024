using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SwordAnimation swordAnimation;
    [SerializeField] DashCollider dashCollider;
    [SerializeField] DashAction dashAction;
    [SerializeField] PlayerMovementStateManager playerMovementStateManager;

    // Components
    DashMovement dashMovement;
    Stab stab;

    private void Start() {
        // Getting components
        dashMovement = GetComponent<DashMovement>();
        stab = GetComponent<Stab>();

        dashCollider.OnContact.AddListener(StabDashContact);
        dashAction.OnDashEnd.AddListener(EndOfDash);
    }

    public void StartStabDash(Vector3 direction) {
        if(playerMovementStateManager.currentState == PlayerMovementState.IDLE && dashMovement.CanDash()) {
            playerMovementStateManager.ChangeState(PlayerMovementState.STAB_DASHING);

            swordAnimation.StartStabDashAnimation();

            dashMovement.PlayerInputDash(direction);
        }
    }
    private void InterruptStabDash() {
        if(playerMovementStateManager.currentState == PlayerMovementState.STAB_DASHING) {
            swordAnimation.EndAnimation();
            playerMovementStateManager.ChangeState(PlayerMovementState.IDLE);
        }
    }

    private void EndOfDash() {
        if (playerMovementStateManager.currentState == PlayerMovementState.STAB_DASHING) {
            swordAnimation.EndAnimation();
            playerMovementStateManager.ChangeState(PlayerMovementState.IDLE);
        }
    }

    private void StabDashContact(GameObject other) {
        if(playerMovementStateManager.currentState == PlayerMovementState.STAB_DASHING) {
            stab.StabContactEffect(other, InterruptStabDash);
        }
    }
}
