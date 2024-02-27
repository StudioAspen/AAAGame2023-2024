using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDashAction : PlayerAction
{
    [Header("References")]
    [SerializeField] DashCollider dashCollider;

    [Header("Dash Variables")]
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;
    [SerializeField] float endDashSpeed;

    [Header("Boosted Variables")]
    [SerializeField] float boostedDashDistance;
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedEndDashSpeed;

    [Header("Other Variables")]
    [SerializeField] float bloodGained;

    // Temp color change
    Renderer render;
    Color holder;

    // Components
    DashMovement dashMovement;
    DashAction dashAction;
    StabContact stabContact;
    MovementModification movementModification;


    //Variables
    bool isDashing = false;

    private void Start() {
        render = GetComponent<Renderer>();
        dashMovement = new DashMovement(transform, GetComponent<Rigidbody>());
        stabContact = GetComponentInChildren<StabContact>();
        movementModification = GetComponentInChildren<MovementModification>();
        dashAction = GetComponent<DashAction>();

        // Setting events
        stabContact.ActivateContactEvent(dashCollider.OnContact, bloodGained);
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
        if(dashMovement.isDashing) {
            dashMovement.InteruptDash();
        }
        stabContact.EndContactEvent();
        OnEndAction.Invoke();
    }
}
