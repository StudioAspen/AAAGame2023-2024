using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashMovement : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;//rigid body of player
    private Collider collider;

    [Header("Stats")]
    public float dashDistance;//How far the dash will go
    public float dashDuration;//How long the dash lasts
    private bool canDash = true;
    private bool isDashing = false;

    [Header("Cooldown")]
    public float dashCooldown;//Cooldown for the dash
    private float dashCdTimer;//Time before you can dash again


    private float dashDurationTimer;//Used to know when dash has ended
    private Vector3 dashVelocity;

    [Header("Temp Keybind")]
    public KeyCode dashKey = KeyCode.E;//dash keybind, E can be changed to preference

    UnityEvent OnDashStart = new UnityEvent();
    UnityEvent OnDashEnd = new UnityEvent();

    //Temperary Grounded
    public LayerMask ground;
    private bool isGrounded;
    public float groundCheckOffset;

    Animator playerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(collider.bounds.min.y - transform.position.y) + groundCheckOffset, ground);
        //Debug.Log(isGrounded);
        if (isGrounded)
        {
            canDash = true;
        }
        if (isDashing)
        {
            float alignment = Vector3.Dot(rb.velocity, dashVelocity);
            if (alignment < 1 && dashDurationTimer > 0)
            {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (dashCdTimer > 0)//if dash is still on cd, count down the timer
        {
            dashCdTimer -= Time.deltaTime;
        }
    }



    public void Dash(Vector3 direction)
    {
        if (dashCdTimer > 0 || !canDash || isDashing)//if dash is still on cooldown or can dash, return
            return;

        dashCdTimer = dashCooldown; // putting dash on cool down
        dashDurationTimer = dashDuration; // setting dash duration
        isDashing = true;
        canDash = false;

        playerAnimator.SetBool("isDashing", true);

        if (direction.magnitude == 0)
        {
            direction = transform.forward;
        }


        dashVelocity = (dashDistance/dashDuration) * direction.normalized; // setting velocity for dashing

        //testing with vizualizations
        Debug.DrawLine(transform.position, transform.position + direction*dashDistance, Color.white, 5);


        OnDashStart.Invoke();


    }

    private void EndDash()
    {
        //rb.velocity = Vector3.zero;
        isDashing = false;
        OnDashEnd.Invoke();

        playerAnimator.SetBool("isDashing", false);
    }

    public void ResetDash()
    {
        canDash = true;
    }

    public void InterruptDash(bool canDash)
    {
        if(canDash)
        {
            ResetDash();
        }
        EndDash();
    }
}
