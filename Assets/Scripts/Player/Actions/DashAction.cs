using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashAction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rb;

    [Header("Events")]
    public UnityEvent OnDashEnd = new UnityEvent();
    
    public bool isDashing = false;

    float dashDurationTimer;// Used to know when dash has ended
    Vector3 dashVelocity; // Velocity used for dashing
    float dragValHolder; // Temperary holder of drag when dashing


    private void FixedUpdate() {
        if (isDashing) {
            UpdateDashing();
        }
    }

    public void Dash(float distance, float duration, Vector3 direction) {
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

        // Vizualization of how far the player SHOULD go
        Debug.DrawLine(rb.position, rb.position + direction * distance, Color.white, 5);
    }
    public void InterruptDash() {
        EndDash();
    }

    private void UpdateDashing() {
        // Allowing dashing as long as the dash direction is no in the same direction as the current velocity
        if (dashDurationTimer > 0) {
            rb.drag = 0;
            rb.velocity = dashVelocity;
        }
        if (dashDurationTimer <= 0) {
            EndDash();
        }
        dashDurationTimer -= Time.fixedDeltaTime;
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
}
