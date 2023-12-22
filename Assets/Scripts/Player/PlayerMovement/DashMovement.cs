using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashMovement : MonoBehaviour
{
    [Header("Movement")]
    public float dashDistance; // How far the dash will go
    public float dashDuration; // How long the dash lasts
    public float dashCooldown; // Cooldown for the dash

    [Header("Boosted Movement")]
    public float boostedDashDistance; // Distance traveled when max overfed
    public float boostedDashDuration; // Duration when max overfed
    public float boostedDashCooldown; // Cooldown when max overfed

    [Header("Events")]
    public UnityEvent OnDashEnd = new UnityEvent();

    [Header("References")]
    [SerializeField] MovementModification movementModification;
    [SerializeField] PlayerPositionCheck playerPositionCheck;
    [SerializeField] DashAction dashAction;


    bool dashAvailable = true;
    float dashCdTimer;//Time before you can dash again

    // Update is called once per frame
    void Update()
    {
        if (dashCdTimer > 0) { //if dash is still on cd, count down the timer 
            dashCdTimer -= Time.deltaTime;
        }
        if (playerPositionCheck.CheckOnGround()) {
            ResetDash();
        }
    }
    public bool CanDash() {
        return dashCdTimer <= 0 && dashAvailable && !dashAction.isDashing;
    }
    public void PlayerInputDash(Vector3 direction)
    {
        if (CanDash()) {
            dashAvailable = false; // Using up the dash
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z); // Only using the horizontal component
            
            // Calculating boosts (all boosts are calculated as a linear interpolation between normal and boost amount given a percentage)
            dashCdTimer = Mathf.Lerp(dashCooldown, boostedDashCooldown, movementModification.boostForAll); 
            float netDashDistance = Mathf.Lerp(dashDistance, boostedDashDistance, movementModification.boostForAll);  
            float netDashDuration = Mathf.Lerp(dashDuration, boostedDashDuration, movementModification.boostForAll); 

            // Starting Dash
            dashAction.Dash(netDashDistance, netDashDuration, horizontalDirection);
        }
    }

    public void ResetDash()
    {
        dashCdTimer = 0;
        dashAvailable = true;
    }

    public void InterruptDash(bool canDashAgain)
    {
        if(canDashAgain) {
            ResetDash();
        }
        dashAction.InterruptDash();
    }
}
