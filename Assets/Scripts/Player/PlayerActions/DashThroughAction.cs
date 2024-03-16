using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashThroughAction : PlayerAction
{
    [Header("Regular")]
    [SerializeField] float dashSpeed; // How fast you move through the dash, this is speed for consistency
    [SerializeField] float endDashSpeedBonus; // How fast you are moving at the end of the dash
    [SerializeField] float initalSpeedScale;  // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)
    [SerializeField] float speedLimit; // The max speed AFTER inital velocity + speed + bonus speed CALCULATION (so this limit applies for both the exit speed and the action itself) 

    [Header("Boosted")]
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float boostedEndDashSpeedBonus;
    [SerializeField] float boostedInitalSpeedScale;
    [SerializeField] float boostsedSpeedLimit;

    [Range(0.0f, 1f)]
    [SerializeField] float stickMag; // How smoothly you transision from your position to the dash, a value of 1 would make you teleport to the center of the object the moment you stab it

    Rigidbody rb;
    Collider playerCollider;
    MovementModification movementModification;

    StabableDashThrough stabable;
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

    public void DashThrough(StabableDashThrough dashThrough, float enemyBonus = 0) {
        dashThrough.CalculateDash(gameObject);

        // Setting variables
        playerCollider.isTrigger = true; // Temp implementation for passing through objects
        isDashing = true;
        stabable = dashThrough;
        distanceTraveled = 0;
        rb.useGravity = false;

        // Speed values
        float currentDashSpeedCalc = movementModification.GetBoost(dashSpeed, boostedDashSpeed, true) + enemyBonus;
        float currentInitalSpeedCalc = rb.velocity.magnitude * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);
        float currentSpeedLimitCalc = movementModification.GetBoost(speedLimit, boostsedSpeedLimit, false);
        currentDashSpeed = Mathf.Min(currentSpeedLimitCalc, currentDashSpeedCalc + currentInitalSpeedCalc);

        OnStartAction.Invoke();
    }

    private void DashThroughUpdate() {
        Vector3 startPos = stabable.gameObject.transform.position;
        Vector3 followPos = startPos + stabable.dashDir.normalized * distanceTraveled;

        rb.position = Vector3.Lerp(rb.position, followPos, stickMag);
        distanceTraveled += currentDashSpeed * Time.fixedDeltaTime;
        if(distanceTraveled > stabable.dashLength) {
            // End Dash speed
            float currentSpeedLimitCalc = movementModification.GetBoost(speedLimit, boostsedSpeedLimit, false);
            float exitSpeed = Mathf.Min(currentSpeedLimitCalc, currentDashSpeed + movementModification.GetBoost(endDashSpeedBonus, boostedEndDashSpeedBonus, true));

            rb.velocity = stabable.dashDir.normalized * exitSpeed;
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