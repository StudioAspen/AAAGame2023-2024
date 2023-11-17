using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashMovement : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;//rigid body of player
    private Collider collider;

    [Header("Base Stats")]
    public float dashDistance;//How far the dash will go
    public float dashDuration;//How long the dash lasts
    public float dashCooldown;//Cooldown for the dash

    [Header("Boosted Stats")]
    public float boostedDuration; // Smallest the duration can be given the boost
    public float boostedCooldown; // Smallest the cooldown can be based on the boost

    private bool dashAvailable = true;
    private bool isDashing = false;
    private float dashCdTimer;//Time before you can dash again
    private float dashDurationTimer;// Used to know when dash has ended
    private Vector3 dashVelocity; // Velocity used for dashing


    public UnityEvent OnDashStart = new UnityEvent();
    public UnityEvent OnDashEnd = new UnityEvent();

    MovementModification movementModification;

    [Header("Ground Check")]
    //Temperary Grounded check
    public LayerMask ground;
    public float groundCheckOffset;


    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        movementModification = GetComponent<MovementModification>();
    }

    private void FixedUpdate()
    {
        if (CheckOnGround())
        {
            dashAvailable = true;
        }

        if (isDashing)
        {
            UpdateDashing();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dashCdTimer > 0)//if dash is still on cd, count down the timer
        {
            dashCdTimer -= Time.deltaTime;
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
        if (direction.magnitude == 0)
        {
            direction = transform.forward;
        }

        isDashing = true;
        dashDurationTimer = duration; // setting dash duration
        dashVelocity = (distance / duration) * direction.normalized; // setting velocity for dashing

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
        if(canDashAgain)
        {
            ResetDash();
        }
        EndDash();
    }

    private void EndDash()
    {
        isDashing = false;
        OnDashEnd.Invoke();
    }

    private void UpdateDashing()
    {
        // Allowing dashing as long as the dash direction is no in the same direction as the current velocity
        float alignment = Vector3.Dot(rb.velocity, dashVelocity); 
        if (alignment < 1 && dashDurationTimer > 0) 
        {
            // Moving the player based on the remaining time for more accurate movement based on distance/duration


            if (dashDurationTimer < Time.fixedDeltaTime)
            {
                rb.MovePosition(rb.position + (dashVelocity * dashDurationTimer));
            }
            else
            {
                rb.MovePosition(rb.position + (dashVelocity * Time.fixedDeltaTime));
            }
        }

        if (dashDurationTimer <= 0)
        {
            EndDash();
        }
        dashDurationTimer -= Time.fixedDeltaTime;
    }
    private bool CheckOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(collider.bounds.min.y - transform.position.y) + groundCheckOffset, ground);
    }
}
