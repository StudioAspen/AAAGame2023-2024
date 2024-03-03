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

    [Header("Boosted Movement")]
    [SerializeField] float boostedDownwardStabAcceleration;
    [SerializeField] float boostedDownwardStabMaxSpeed;
    [SerializeField] float boostedInitalSpeedScale;

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
        Vector3 addedVelocity = Vector3.down * movementModification.GetBoost(downwardStabAcceleration, boostedDownwardStabMaxSpeed, true);
        Vector3 maxVelocity = Vector3.down * movementModification.GetBoost(downwardStabMaxSpeed, boostedDownwardStabMaxSpeed, true);
        float velocityAlignment = Vector3.Dot(startVelocity, maxVelocity);
        maxVelocity = maxVelocity + (velocityAlignment*maxVelocity.normalized);

        Vector3 currentVerticalVelocity = new Vector3(0, rb.velocity.y, 0);

        // Applying vertical movement if the speed is higher than the max velocity
        float alignment = Vector3.Dot(currentVerticalVelocity / maxVelocity.magnitude, maxVelocity / maxVelocity.magnitude);
        if (alignment < 1) {
            rb.AddForce(addedVelocity, ForceMode.VelocityChange);
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
