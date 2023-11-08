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

    [Header("Boost Values")]
    private float dashCdTimer;//Time before you can dash again
    private bool dashAvailable = true;
    private bool isDashing = false;

    private float dashDurationTimer;// Used to know when dash has ended
    private Vector3 dashVelocity; // Velocity used for dashing


    UnityEvent OnDashStart = new UnityEvent();
    UnityEvent OnDashEnd = new UnityEvent();

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

        if(!TryGetComponent<MovementModification>(out movementModification))
        {
            Debug.Log("Cannot find movement modification for dashMovement");
        }
        else
        {
            movementModification.OnModifyMovement.AddListener(ApplyMovementModification);
        }
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
        if (dashCdTimer <= 0 && dashAvailable)
        {
            dashAvailable = false; // Using up the dash
            dashCdTimer = dashCooldown; // putting dash on cool down
            Dash(dashDistance, dashDuration, direction);
        }
    }
    public void Dash(float distance, float duration, Vector3 direction)
    {
        if (!isDashing)
        {
            dashDurationTimer = dashDuration; // setting dash duration
            isDashing = true;

            // Default value for no direction given
            if (direction.magnitude == 0)
            {
                direction = transform.forward;
            }

            dashVelocity = (distance / duration) * direction.normalized; // setting velocity for dashing
            OnDashStart.Invoke();

            //testing with vizualizations
            Debug.DrawLine(transform.position, transform.position + direction * dashDistance, Color.white, 5);
        }
    }

    private void EndDash()
    {
        //rb.velocity = Vector3.zero;
        isDashing = false;
        OnDashEnd.Invoke();
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
    private void ApplyMovementModification()
    {

    }
    private bool CheckOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(collider.bounds.min.y - transform.position.y) + groundCheckOffset, ground);
    }
}
