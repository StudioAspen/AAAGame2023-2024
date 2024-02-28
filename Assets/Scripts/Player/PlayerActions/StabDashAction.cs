using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDashAction : PlayerAction
{
    [Header("References")]
    [SerializeField] DashCollider dashCollider;

    [Header("Dash Variables")]
    [SerializeField] float dashSpeed; // The speed on top of the player velocity
    [SerializeField] float dashDuration; // How long the dash takes
    [SerializeField] float endDashSpeedBonus; // How fast the player is going at the end of the dash

    [Header("Boosted Variables")]
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedEndDashSpeedBonus;

    [Header("Other Variables")]
    [SerializeField] float bloodGained; // How much blood is gained when striking something stabable

    // Temp color change
    Renderer render;
    Color holder;

    // Components
    Rigidbody rb;
    DashMovement dashMovement;
    DashAction dashAction;
    StabContact stabContact;
    MovementModification movementModification;


    //Variables
    bool isDashing = false;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        render = GetComponent<Renderer>();
        dashMovement = new DashMovement(transform, GetComponent<Rigidbody>());
        stabContact = GetComponentInChildren<StabContact>();
        movementModification = GetComponentInChildren<MovementModification>();
        dashAction = GetComponent<DashAction>();

        // Setting events
        dashMovement.OnDashEnd.AddListener(EndAction);

        // Temp holder
        holder = render.material.color;
    }

    private void FixedUpdate() {
        dashMovement.UpdateDashing();
    }

    public void StabDashInput(Vector3 direction) {
        if(!isDashing) {
            isDashing = true;
            render.material.color = Color.red;
            dashAction.ConsumeDash();
            stabContact.ActivateContactEvent(dashCollider.OnContact, bloodGained);

            // Calculating boosted variables
            float currentDashSpeed = movementModification.GetBoost(dashSpeed, boostedDashSpeed, true);
            float currentDashDuration = movementModification.GetBoost(dashDuration, boostedDashDuration, true);
            float currentEndDashSpeedBonus = movementModification.GetBoost(endDashSpeedBonus, boostedEndDashSpeedBonus, true);

            float velocityAlignment = Vector3.Dot(rb.velocity, direction);
            dashMovement.Dash(velocityAlignment + currentDashSpeed, currentDashDuration, direction, velocityAlignment + currentDashSpeed + currentEndDashSpeedBonus);
        }
    }

    public override void EndAction() {
        isDashing = false;
        render.material.color = holder;
        if(dashMovement.isDashing) {
            dashMovement.InteruptDash();
        }
        stabContact.EndContactEvent();
        OnEndAction.Invoke();
    }
}
