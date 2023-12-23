using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SlashDash : MonoBehaviour {
    [Header("References")]
    [SerializeField] DashCollider dashCollider;
    [SerializeField] SwordAnimation swordAnimation;
    [SerializeField] DashAction dashAction;
    [SerializeField] PlayerMovementStateManager playerMovementStateManager;

    // Components
    DashMovement dashMovement;
    Slash slash;

    private void Start() {
        dashMovement = GetComponent<DashMovement>();
        slash = GetComponent<Slash>();

        dashCollider.OnContact.AddListener(SlashDashContact);
        dashAction.OnDashEnd.AddListener(EndOfDash);
    }

    public void StartSlashDash(Vector3 direction) {
        if (playerMovementStateManager.currentState == PlayerMovementState.IDLE && dashMovement.CanDash()) {
            playerMovementStateManager.ChangeState(PlayerMovementState.SLASH_DASHING);

            swordAnimation.StartSlashDashAnimation();
            dashMovement.PlayerInputDash(direction);
        }
    }

    private void InterruptSlashDash() {
        if (playerMovementStateManager.currentState == PlayerMovementState.SLASH_DASHING) {
            swordAnimation.EndAnimation();
            playerMovementStateManager.ChangeState(PlayerMovementState.IDLE);
        }
    }
    
    private void EndOfDash() {
        if (playerMovementStateManager.currentState == PlayerMovementState.SLASH_DASHING) {
            swordAnimation.EndAnimation();
            playerMovementStateManager.ChangeState(PlayerMovementState.IDLE);
        }
    }
    private void SlashDashContact(GameObject other) {
        if (playerMovementStateManager.currentState == PlayerMovementState.SLASH_DASHING) {
            slash.SlashContactEffect(other, InterruptSlashDash);
        }
    }
}
