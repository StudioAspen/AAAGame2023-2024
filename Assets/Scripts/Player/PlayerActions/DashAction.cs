using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashAction : PlayerAction
{
    [Header("Movement")]
    [SerializeField] float dashDistance; // How far the dash will go
    [SerializeField] float dashDuration; // How long the dash lasts
    [SerializeField] float dashCooldown; // Cooldown for the dash

    [Header("Boosted Movement")]
    [SerializeField] float boostedDashDistance; // Distance traveled when max overfed
    [SerializeField] float boostedDashDuration; // Duration when max overfed
    [SerializeField] float boostedDashCooldown; // Cooldown when max overfed

    bool dashAvailable = true;
    float dashCdTimer;//Time before you can dash again

    // References
    MovementModification movementModification;
    PlayerPositionCheck playerPositionCheck;
    DashMovement dashMovement;

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        movementModification = GetComponent<MovementModification>();
        playerPositionCheck = GetComponent<PlayerPositionCheck>();
        dashMovement = new DashMovement(transform, GetComponent<Rigidbody>());
    }

    // Update is called once per frame
    void Update()
    {
        if (dashCdTimer > 0) { //if dash is still on cd, count down the timer 
            dashCdTimer -= Time.deltaTime;
        }
        if (playerPositionCheck.CheckOnGround()) {
            ResetDash();
        }
        dashMovement.UpdateDashing();
    }
    public void DashInput(Vector3 direction)
    {
        if (dashCdTimer <= 0 && dashAvailable && !dashMovement.isDashing) {
            dashAvailable = false; // Using up the dash
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z); // Only using the horizontal component
            
            // Calculating boosts (all boosts are calculated as a linear interpolation between normal and boost amount given a percentage)
            dashCdTimer = Mathf.Lerp(dashCooldown, boostedDashCooldown, movementModification.boostForAll); 
            float netDashDistance = Mathf.Lerp(dashDistance, boostedDashDistance, movementModification.boostForAll);  
            float netDashDuration = Mathf.Lerp(dashDuration, boostedDashDuration, movementModification.boostForAll);

            // Starting Dash
            dashMovement.Dash(netDashDistance, netDashDuration, horizontalDirection);
        }
    }

    public void ResetDash()
    {
        dashCdTimer = 0;
        dashAvailable = true;
    }

    public override void EndAction() {
        //Ending dash
        dashMovement.EndDash();
        OnEndAction.Invoke();
    }
}
