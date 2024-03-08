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
    [SerializeField] float initalSpeedScale;  // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)
    [SerializeField] float speedLimit; // The max speed AFTER inital velocity + speed + bonus speed CALCULATION (so this limit applies for both the exit speed and the action itself) 

    [Header("Boosted Variables")]
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedEndDashSpeedBonus;
    [SerializeField] float boostedInitalSpeedScale;
    [SerializeField] float boostsedSpeedLimit;

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
            float currentVelocity = rb.velocity.magnitude * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);

            // Limiting Speed
            float currentMaxSpeed = movementModification.GetBoost(speedLimit, boostsedSpeedLimit, false);
            float appliedDashSpeed = Mathf.Min(currentMaxSpeed, currentVelocity + currentDashSpeed);
            float appliedExitSpeed = Mathf.Min(currentMaxSpeed, appliedDashSpeed + currentEndDashSpeedBonus);

            dashMovement.Dash(appliedDashSpeed, currentDashDuration, direction, appliedExitSpeed);
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
