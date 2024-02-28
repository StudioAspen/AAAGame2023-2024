using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashAction : PlayerAction
{
    [Header("Movement")]
    [SerializeField] float dashSpeed; // The speed player will add onto current speed
    [SerializeField] float dashDuration; // How long the dash lasts
    [SerializeField] float dashCooldown; // Cooldown between consecutive dashes
    [SerializeField] float endDashSpeedBonus; // Speed at the end of the dash

    [Header("Boosted Movement")]
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedDashCooldown;
    [SerializeField] float boostedEndDashSpeedBonus;

    bool dashAvailable = true;
    float dashCdTimer;// Time before you can dash again
    public float timer = 0; // Used for starting slash/stab dash

    // References
    Rigidbody rb;
    MovementModification movementModification;
    PlayerPositionCheck playerPositionCheck;
    DashMovement dashMovement;

    // Temp for visual clarity
    Renderer render;
    Color holder;

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponentInChildren<MovementModification>();
        playerPositionCheck = GetComponentInChildren<PlayerPositionCheck>();
        dashMovement = new DashMovement(transform, rb);
        dashMovement.OnDashEnd.AddListener(EndAction);


        // Temp for visual clarity
        render = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // if dash is still on cd, count down the timer 
        if (dashCdTimer > 0) { 
            dashCdTimer -= Time.deltaTime;
        }

        // Ground Check
        if (playerPositionCheck.CheckOnGround()) {
            ResetDash();
        }

        if(dashMovement.isDashing) {
            timer += Time.deltaTime;
        }
    }
    private void FixedUpdate() {
        dashMovement.UpdateDashing();
    }
    // Input for this actio
    public void DashInput(Vector3 direction)
    {
        // Temp
        holder = render.material.color;
        render.material.color = Color.blue;

        timer = 0;
        dashAvailable = false; // Using up the dash
        
        // Calculating boosts (all boosts are calculated as a linear interpolation between normal and boost amount given a percentage)
        dashCdTimer = movementModification.GetBoost(dashCooldown, boostedDashCooldown, true); 
        float currentDashSpeed = movementModification.GetBoost(dashSpeed, boostedDashSpeed, true);  
        float currentDashDuration = movementModification.GetBoost(dashDuration, boostedDashDuration, true);
        float currentEndDashSpeedBonus = movementModification.GetBoost(endDashSpeedBonus, boostedEndDashSpeedBonus, true);

        float velocityAlignment = Vector3.Dot(rb.velocity, direction);
        dashMovement.Dash(velocityAlignment + currentDashSpeed, currentDashDuration, direction, velocityAlignment + currentDashSpeed + currentEndDashSpeedBonus);
    }

    // Resets the dash allowing player to dash again
    public void ResetDash()
    {
        dashCdTimer = 0;
        dashAvailable = true;
    }

    public void ConsumeDash() {
        dashAvailable = false;
    }

    // Checking if player can perform a dash
    public bool CanPerformDash() {
        return dashCdTimer <= 0 && dashAvailable && !dashMovement.isDashing;
    }

    // End this action
    public override void EndAction() {
        render.material.color = holder;

        //Ending dash
        if (dashMovement.isDashing) {
            dashMovement.InteruptDash();
        }
        OnEndAction.Invoke();
    }
}
