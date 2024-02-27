using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashAction : PlayerAction
{
    [Header("Movement")]
    [SerializeField] float dashDistance; // How far the dash will go
    [SerializeField] float dashDuration; // How long the dash lasts
    [SerializeField] float dashCooldown; // Cooldown between consecutive dashes
    [SerializeField] float endDashSpeed; // Speed at the end of the dash

    [Header("Boosted Movement")]
    [SerializeField] float boostedDashDistance;
    [SerializeField] float boostedDashDuration;
    [SerializeField] float boostedDashCooldown;
    [SerializeField] float boostedEndDashSpeed;

    bool dashAvailable = true;
    float dashCdTimer;// Time before you can dash again
    public float timer = 0; // Used for starting slash/stab dash

    // References
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
        movementModification = GetComponentInChildren<MovementModification>();
        playerPositionCheck = GetComponentInChildren<PlayerPositionCheck>();
        dashMovement = new DashMovement(transform, GetComponent<Rigidbody>());
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
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z); // Only using the horizontal component
        
        // Calculating boosts (all boosts are calculated as a linear interpolation between normal and boost amount given a percentage)
        dashCdTimer = movementModification.GetBoost(dashCooldown, boostedDashCooldown, true); 
        float currentDashDistance = movementModification.GetBoost(dashDistance, boostedDashDistance, true);  
        float currentDashDuration = movementModification.GetBoost(dashDuration, boostedDashDuration, true);
        float currentEndDashSpeed = movementModification.GetBoost(endDashSpeed, boostedEndDashSpeed, true);

        // Starting Dash
        dashMovement.Dash(currentDashDistance, currentDashDuration, horizontalDirection, currentEndDashSpeed);
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
