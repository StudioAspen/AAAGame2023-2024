using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SlashDashAction : PlayerAction {
    [Header("References")]
    [SerializeField] DashCollider dashCollider;

    [Header("Dash Variables")]
    [SerializeField] float dashDistance; // How far the dash goes
    [SerializeField] float dashDuration; // How long the dash lasts
    [SerializeField] float endDashSpeed; // How fast the player is moving at the end of the dash

    [Header("Boosted Variables")]
    [SerializeField] float boostedDashDistance;
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedEndDashSpeed;

    [Header("Other Variables")]
    [SerializeField] float bloodGained; // How much blood the player gains when striking something slashable

    // Temp color change
    Renderer render;
    Color holder;

    // Components
    DashMovement dashMovement;
    DashAction dashAction;
    SlashContact slashContact;
    MovementModification movementModification;

    //Variables
    bool isDashing = false;

    private void Start() {
        render = GetComponent<Renderer>();
        dashMovement = new DashMovement(transform, GetComponent<Rigidbody>());
        slashContact = GetComponentInChildren<SlashContact>();
        movementModification = GetComponentInChildren<MovementModification>();
        dashAction = GetComponent<DashAction>();


        dashMovement.OnDashEnd.AddListener(EndAction);

        // Temp holder
        holder = render.material.color;
    }

    private void FixedUpdate() {
        dashMovement.UpdateDashing();
    }

    public void SlashDashInput(Vector3 direction) {
        if (!isDashing) {
            isDashing = true;
            dashAction.ConsumeDash();
            slashContact.ActivateContactEvent(dashCollider.OnContact, bloodGained);

            render.material.color = Color.red;

            // Calculating boosted variables
            float currentDashDuration = movementModification.GetBoost(dashDuration, boostedDashDuration, true);
            float currentDashDistance = movementModification.GetBoost(dashDistance, boostedDashDistance, true);
            float currentEndDashSpeed = movementModification.GetBoost(endDashSpeed, boostedEndDashSpeed, true);

            dashMovement.Dash(currentDashDistance, currentDashDuration, direction, currentEndDashSpeed);
        }
    }
    

    public override void EndAction() {
        isDashing = false;
        render.material.color = holder;
        if (dashMovement.isDashing) {
            dashMovement.InteruptDash();
        }
        slashContact.EndContactEvent();
        OnEndAction.Invoke();
    }
}
