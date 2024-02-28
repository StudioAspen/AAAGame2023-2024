using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThroughAction : PlayerAction
{
    [Header("Regular")]
    [SerializeField] float dashSpeed; // How fast you move through the dash, this is speed for consistency
    [SerializeField] float endDashSpeedBonus; // How fast you are moving at the end of the dash
    [SerializeField] float initalSpeedScale;  // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)

    [Header("Boosted")]
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float boostedEndDashSpeedBonus;
    [SerializeField] float boostedInitalSpeedScale;

    [Range(0.0f, 1f)]
    [SerializeField] float stickMag; // How smoothly you transision from your position to the dash, a value of 1 would make you teleport to the center of the object the moment you stab it

    Rigidbody rb;
    Collider playerCollider;
    MovementModification movementModification;

    StabableEnviornment stabable;
    float distanceTraveled = 0;
    bool isDashing = false;
    float currentDashSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        movementModification = GetComponentInChildren<MovementModification>();
    }

    private void FixedUpdate() {
        if(isDashing) {
            DashThroughUpdate();
        }
    }

    public void DashThrough(StabableEnviornment stabableEnviornment) {
        playerCollider.isTrigger = true; // Temp implementation for passing through objects
        isDashing = true;
        stabable = stabableEnviornment;
        distanceTraveled = 0;
        rb.useGravity = false;
        currentDashSpeed = movementModification.GetBoost(dashSpeed, boostedDashSpeed, true);
        currentDashSpeed += rb.velocity.magnitude * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);
    }

    private void DashThroughUpdate() {
        Vector3 startPos = stabable.gameObject.transform.position;
        Vector3 followPos = startPos + stabable.dashDir.normalized * distanceTraveled;

        rb.position = Vector3.Lerp(rb.position, followPos, stickMag);
        distanceTraveled += currentDashSpeed * Time.fixedDeltaTime;
        if(distanceTraveled > stabable.dashLength) {
            // End Dash speed
            rb.velocity = stabable.dashDir.normalized * (currentDashSpeed + movementModification.GetBoost(endDashSpeedBonus, boostedEndDashSpeedBonus, true));
            EndAction();
        }
    }

    public override void EndAction() {
        playerCollider.isTrigger = false;
        isDashing = false;
        rb.useGravity = true;
        OnEndAction.Invoke();
    }
}