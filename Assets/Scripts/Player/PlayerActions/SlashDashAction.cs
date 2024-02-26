using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SlashDashAction : PlayerAction {
    [Header("References")]
    [SerializeField] DashCollider dashCollider;

    [Header("Dash Variables")]
    [SerializeField] float dashDuration;
    [SerializeField] float dashDistance;

    [Header("Boosted Variables")]
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedDashDistance;

    [Header("Other Variables")]
    [SerializeField] float bloodGained;

    // Temp color change
    Renderer renderer;
    Color holder;

    // Components
    Rigidbody rb;
    DashMovement dashMovement;
    SlashContact slashContact;
    MovementModification movementModification;

    //Variables
    bool isDashing = false;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        dashMovement = new DashMovement(transform, rb);
        slashContact = GetComponentInChildren<SlashContact>();
        movementModification = GetComponentInChildren<MovementModification>();

        slashContact.ActivateContactEvent(dashCollider.OnContact, EndAction, bloodGained);
    }

    public void SlashDashInput(Vector3 direction) {
        if (!isDashing) {
            isDashing = true;

            renderer.material.color = Color.red;

            // Calculating boosted variables
            float currentDashDuration = movementModification.GetBoost(dashDuration, boostedDashDuration, true);
            float currentDashDistance = movementModification.GetBoost(dashDistance, boostedDashDistance, true);

            dashMovement.Dash(currentDashDistance, currentDashDuration, direction);
        }
    }
    

    public override void EndAction() {
        isDashing = false;
        renderer.material.color = holder;
        slashContact.EndContactEvent();
        OnEndAction.Invoke();
    }
}
