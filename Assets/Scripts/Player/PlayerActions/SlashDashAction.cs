using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SlashDashAction : PlayerAction {
    [Header("References")]
    [SerializeField] DashCollider dashCollider;

    [Header("Dash Variables")]
    [SerializeField] float dashSpeed; // How far the dash goes
    [SerializeField] float dashDuration; // How long the dash lasts
    [SerializeField] float endDashSpeedBonus; // How fast the player is moving at the end of the dash
    [SerializeField] float initalSpeedScale;  // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)
    [SerializeField] float speedLimit; // The max speed AFTER inital velocity + speed + bonus speed CALCULATION (so this limit applies for both the exit speed and the action itself) 

    [Header("Boosted Variables")]
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedEndDashSpeedBonus;
    [SerializeField] float boostedInitalSpeedScale;
    [SerializeField] float boostsedSpeedLimit;

    [Header("Other Variables")]
    [SerializeField] float bloodGained; // How much blood the player gains when striking something slashable

    // Temp color change
    Renderer render;
    Color holder;

    // Components
    Rigidbody rb;
    DashMovement dashMovement;
    DashAction dashAction;
    SlashContact slashContact;
    MovementModification movementModification;

    //Variables
    bool isDashing = false;

    private void Start() {
        rb = GetComponent<Rigidbody>();
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

            // Calculating boosts (all boosts are calculated as a linear interpolation between normal and boost amount given a percentage)
            float currentDashSpeed = movementModification.GetBoost(dashSpeed, boostedDashSpeed, true);
            float currentDashDuration = movementModification.GetBoost(dashDuration, boostedDashDuration, true);
            float currentEndDashSpeedBonus = movementModification.GetBoost(endDashSpeedBonus, boostedEndDashSpeedBonus, true);
            float currentVelocity = rb.velocity.magnitude * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);

            // Limiting Speed
            float currentMaxSpeed = movementModification.GetBoost(speedLimit, boostsedSpeedLimit, false);
            float appliedDashSpeed = Mathf.Min(currentMaxSpeed, currentVelocity + currentDashSpeed);
            float appliedExitSpeed = Mathf.Min(currentMaxSpeed, appliedDashSpeed + currentEndDashSpeedBonus);

            dashMovement.Dash(appliedDashSpeed, currentDashDuration, direction, appliedExitSpeed);

            OnStartAction.Invoke();
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
