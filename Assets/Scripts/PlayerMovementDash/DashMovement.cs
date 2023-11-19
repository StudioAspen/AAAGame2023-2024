using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashMovement : MonoBehaviour
{
    // References
    Rigidbody rb;//rigid body of player
    MovementModification movementModification;
    GroundCheck groundCheck;

    [Header("Base Stats")]
    public float dashDistance; // How far the dash will go
    public float dashDuration; // How long the dash lasts
    public float dashCooldown; // Cooldown for the dash

    [Header("Boosted Stats")]
    public float boostedDashDistance; // Distance traveled when max overfed
    public float boostedDashDuration; // Duration when max overfed
    public float boostedDashCooldown; // Cooldown when max overfed

    bool dashAvailable = true;
    bool isDashing = false;
    float dashCdTimer;//Time before you can dash again
    float dashDurationTimer;// Used to know when dash has ended
    Vector3 dashVelocity; // Velocity used for dashing
    float dragValHolder; // Temperary holder of drag when dashing

    [Header("Events")]
    public UnityEvent OnDashEnd = new UnityEvent();


    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponent<MovementModification>();
        groundCheck = GetComponent<GroundCheck>();
    }

    private void FixedUpdate() {
        if (isDashing) {
            UpdateDashing();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dashCdTimer > 0) { //if dash is still on cd, count down the timer 
            dashCdTimer -= Time.deltaTime;
        }
        if (groundCheck.CheckOnGround()) {
            ResetDash();
        }
    }
    public void PlayerInputDash(Vector3 direction)
    {
        if (dashCdTimer <= 0 && dashAvailable && !isDashing) {
            dashAvailable = false; // Using up the dash
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z); // Only using the horizontal component
            
            // Calculating boosts (all boosts are calculated as a linear interpolation between normal and boost amount given a percentage)
            dashCdTimer = Mathf.Lerp(dashCooldown, boostedDashCooldown, movementModification.boostForAll); 
            float netDashDistance = Mathf.Lerp(dashDistance, boostedDashDistance, movementModification.boostForAll);  
            float netDashDuration = Mathf.Lerp(dashDuration, boostedDashDuration, movementModification.boostForAll); 

            // Starting Dash
            Dash(netDashDistance, netDashDuration, horizontalDirection);
        }
    }
    public void Dash(float distance, float duration, Vector3 direction)
    {
        // Default value for no direction given
        if (direction.magnitude == 0) {
            direction = transform.forward;
        }

        // Setting variables
        isDashing = true;
        dashDurationTimer = duration; // starting dash duration timer
        dashVelocity = (distance / duration) * direction.normalized; // setting velocity for dashing

        // Set up for physics variables
        dragValHolder = rb.drag;
        rb.useGravity = false;
        rb.velocity = dashVelocity;

        // Vizualization of how far the player SHOULD
        Debug.DrawLine(rb.position, rb.position + direction * distance, Color.white, 5);
    }

    public void ResetDash()
    {
        dashAvailable = true;
    }

    public void InterruptDash(bool canDashAgain)
    {
        if(canDashAgain) {
            ResetDash();
        }
        EndDash();
    }

    private void EndDash() {
        //Rstoring movement variables
        rb.drag = dragValHolder;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;

        //Ending dash
        isDashing = false;
        OnDashEnd.Invoke();
    }

    private void UpdateDashing()
    {
        // Allowing dashing as long as the dash direction is no in the same direction as the current velocity
        if (dashDurationTimer > 0) 
        {
            rb.drag = 0;
            rb.velocity = dashVelocity;
        }

        if (dashDurationTimer <= 0) {
            EndDash();
        }
        dashDurationTimer -= Time.fixedDeltaTime;
    }
}
