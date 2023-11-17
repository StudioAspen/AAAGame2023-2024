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
    public float dashDistance;//How far the dash will go
    public float dashDuration;//How long the dash lasts
    public float dashCooldown;//Cooldown for the dash

    [Header("Boosted Stats")]
    public float boostedDuration; // Smallest the duration can be given the boost
    public float boostedCooldown; // Smallest the cooldown can be based on the boost

    bool dashAvailable = true;
    bool isDashing = false;
    float dashCdTimer;//Time before you can dash again
    float dashDurationTimer;// Used to know when dash has ended
    Vector3 dashVelocity; // Velocity used for dashing

    private float dragValHolder;

    [Header("Events")]
    public UnityEvent OnDashStart = new UnityEvent();
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
        if (dashCdTimer <= 0 && dashAvailable && !isDashing)
        {
            dashAvailable = false; // Using up the dash
            dashCdTimer = Mathf.Lerp(dashCooldown, boostedCooldown, movementModification.boostForAll); // putting dash on cool down considering boost
            float netDashDuration = Mathf.Lerp(dashDuration, boostedDuration, movementModification.boostForAll); // calculating boost for durations
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z); // Only using the horizontal component
            Dash(dashDistance, netDashDuration, horizontalDirection);
        }
    }
    public void Dash(float distance, float duration, Vector3 direction)
    {
        // Default value for no direction given
        if (direction.magnitude == 0) {
            direction = transform.forward;
        }

        isDashing = true;
        dashDurationTimer = duration; // starting dash duration timer
        dashVelocity = (distance / duration) * direction.normalized; // setting velocity for dashing

        //TESTING HEREEEEEEEEEEE
        dragValHolder = rb.drag;
        rb.velocity = Vector3.zero;


        OnDashStart.Invoke();

        //testing with vizualizations
        Debug.DrawLine(transform.position, transform.position + direction * distance, Color.white, 5);
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
        rb.velocity = Vector3.zero;
        rb.drag = dragValHolder;

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
        dashDurationTimer -= Time.deltaTime;
    }
}
