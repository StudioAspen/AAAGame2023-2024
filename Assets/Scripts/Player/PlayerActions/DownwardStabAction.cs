using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardStabAction : PlayerAction {

    [Header("References")]
    [SerializeField] SwordMovement swordMovement;

    [Header("Movement")]
    [SerializeField] float downwardStabAcceleration; // How fast the player accelerates to max speed while stabbing
    [SerializeField] float downwardStabMaxSpeed; // The max speed while downward stabbing
    [SerializeField] float initalSpeedScale; // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)
    [SerializeField] float speedLimit; // The limit speed AFTER inital velocity + speed CALCULATION (so this limit applies for both just the max speed, acceleration is unaffected) 

    [Header("Boosted Movement")]
    [SerializeField] float boostedDownwardStabAcceleration;
    [SerializeField] float boostedDownwardStabMaxSpeed;
    [SerializeField] float boostedInitalSpeedScale;
    [SerializeField] float boostedSpeedLimit;


    [Header("Other Variables")]
    [SerializeField] float pressDownTime; // Amount of time you need to hold down the stab button before starting downward stab
    [SerializeField] float bloodGain; // Amount of blood gained when striking a stabable object

    //variables for downward stab
    float stabButtonTimer = 0.0f;
    bool canDownwardStab = true;
    bool isStabing = false;
    Vector3 startVelocity;

    // Components
    Rigidbody rb;
    PlayerPositionCheck playerPositionCheck;
    StabAction stabAction;
    MovementModification movementModification;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        playerPositionCheck = GetComponentInChildren<PlayerPositionCheck>();
        stabAction = GetComponent<StabAction>();
        movementModification = GetComponentInChildren<MovementModification>();

        swordMovement.OnContact.AddListener(DownwardStabContact);
    }

    private void FixedUpdate() {
        if(isStabing) {
            DownwardStabMovementUpdate();
        }
    }

    private void Update() {
        //if grounded can perform downward stab
        canDownwardStab = !playerPositionCheck.CheckOnGround();
    }

    public void DownwardStabInputUpdate() {
        if (canDownwardStab && !isStabing) {
            stabButtonTimer += Time.deltaTime;

            // Starting downward stab as long as the duration
            if (stabButtonTimer >= pressDownTime) {
                stabAction.EndAction();
                isStabing = true;
                startVelocity = rb.velocity * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);

                PlayerActionManager manager = GetComponentInChildren<PlayerActionManager>();
                manager.ChangeAction(this);
                swordMovement.DownwardAttackPosition();
            }
        }
    }

    public void DownwardStabInputRelease() {
        stabButtonTimer = 0;

        if (isStabing) {
            EndAction();
        }
    }
    private void DownwardStabMovementUpdate() {
        // Calculating inital variables
        float addedSpeed = movementModification.GetBoost(downwardStabAcceleration, boostedDownwardStabMaxSpeed, true);
        float maxSpeed = movementModification.GetBoost(downwardStabMaxSpeed, boostedDownwardStabMaxSpeed, true);

        maxSpeed += startVelocity.magnitude;

        //Limiting speed
        float currentMaxSpeed = movementModification.GetBoost(speedLimit, boostedSpeedLimit, false);
        maxSpeed = Math.Min(currentMaxSpeed, maxSpeed);

        // Applying vertical movement if the speed is higher than the max velocity
        if (-rb.velocity.y < maxSpeed) {
            rb.velocity += Vector3.down*addedSpeed;
        }
    }

    private void DownwardStabContact(Collider other) {
        if(isStabing) {
            if(other.TryGetComponent(out DownwardStabEffect downwardStabEffect)) {
                downwardStabEffect.TriggerEffect();
            }
            if(other.TryGetComponent(out BouncePadEffect bouncePad)) {
                // Setting new velocity
                float newVerticalSpeed = rb.velocity.y * bouncePad.initalSpeedScale;
                newVerticalSpeed = MathF.Max(newVerticalSpeed, bouncePad.minimumExitSpeed); // Setting speed to minimum
                rb.velocity = new Vector3(rb.velocity.x, newVerticalSpeed, rb.velocity.z);
            }
            EndAction();
        }
    }

    public override void EndAction() {
        isStabing = false;
        swordMovement.EndAttackPosition();
        OnEndAction.Invoke();
    }
}
