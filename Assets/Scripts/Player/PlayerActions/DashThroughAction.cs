using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThroughAction : PlayerAction
{
    [SerializeField] float dashSpeed;
    [SerializeField] float boostedDashSpeed;
    
    [Range(0.0f, 1f)]
    [SerializeField] float stickMag;

    Rigidbody rb;
    Collider collider;
    MovementModification movementModification;

    StabableEnviornment stabable;
    float distanceTraveled = 0;
    bool isDashing = false;
    float currentDashSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        movementModification = GetComponentInChildren<MovementModification>();
    }

    private void FixedUpdate() {
        if(isDashing) {
            DashThroughUpdate();
        }
    }

    public void DashThrough(StabableEnviornment stabableEnviornment) {
        collider.isTrigger = true; // Temp implementation for passing through objects
        isDashing = true;
        stabable = stabableEnviornment;
        distanceTraveled = 0;
        rb.useGravity = false;
        currentDashSpeed = movementModification.GetBoost(dashSpeed, boostedDashSpeed, true);
    }

    private void DashThroughUpdate() {
        Vector3 startPos = stabable.gameObject.transform.position;
        Vector3 followPos = startPos + stabable.dashDir.normalized * distanceTraveled;

        rb.position = Vector3.Lerp(rb.position, followPos, stickMag);
        distanceTraveled += currentDashSpeed * Time.fixedDeltaTime;
        if(distanceTraveled > stabable.dashLength) {
            rb.velocity = stabable.dashDir.normalized * currentDashSpeed;
            EndAction();
        }
    }

    public override void EndAction() {
        collider.isTrigger = false;
        isDashing = false;
        rb.useGravity = true;
        OnEndAction.Invoke();
    }
}