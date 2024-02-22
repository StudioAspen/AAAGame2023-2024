using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashAction {
    // References needed
    Rigidbody rb;
    Transform transform;

    // temperary variables
    public UnityEvent OnDashEnd = new UnityEvent();
    float dashDurationTimer;
    Vector3 dashVelocity;
    float dragValHolder;
    bool isDashing = false;


    DashAction(Transform _transform, Rigidbody _rb) {
        transform = _transform;
        rb = _rb;
    }

    public void Dash(float distance, float duration, Vector3 direction) {
        // Default value for no direction given
        if (direction.magnitude == 0) {
            direction = transform.forward;
        }

        // Setting variables
        dashDurationTimer = duration; // starting dash duration timer
        dashVelocity = (distance / duration) * direction.normalized; // setting velocity for dashing

        // Set up for physics variables
        isDashing = true;
        dragValHolder = rb.drag;
        rb.useGravity = false;
        rb.velocity = dashVelocity;

        // Vizualization of how far the player SHOULD go
        Debug.DrawLine(rb.position, rb.position + direction * distance, Color.white, 5);
    }

    public void UpdateDashing() {
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
    public void EndDash() {
        //Rstoring movement variables
        rb.drag = dragValHolder;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;

        isDashing = false;

        OnDashEnd.Invoke();
    }
}
