using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDashAction : PlayerAction
{
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
    DashMovement dashMovement;
    StabContact stabContact;
    MovementModification movementModification;


    //Variables
    bool isDashing = false;

    private void Start() {
        // Getting components
        stabContact = GetComponentInChildren<StabContact>();
        dashMovement = new DashMovement(transform, GetComponent<Rigidbody>());
        renderer = GetComponent<Renderer>();

        // Setting events
        stabContact.ActivateContactEvent(dashCollider.OnContact, EndAction, bloodGained);
        dashMovement.OnDashEnd.AddListener(EndAction);

        // Temp holder
        holder = renderer.material.color;
    }

    public void StabDashInput(Vector3 direction) {
        if(!isDashing) {
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
        stabContact.EndContactEvent();
        OnEndAction.Invoke();
    }
}
