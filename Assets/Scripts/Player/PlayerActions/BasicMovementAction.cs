using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovementAction : PlayerAction
{
    //Components
    PlayerPositionCheck playerPositionCheck;
    Rigidbody rb;
    MovementModification movementModification;

    [Header("Ground Variables")]
    [SerializeField] float groundAcceleration;
    [SerializeField] float maxGroundSpeed;
    [SerializeField] float groundDrag;

    [Header("Boosted Ground")]
    [SerializeField] float boostedGroundAcceleration;
    [SerializeField] float boostedMaxGroundSpeed;
    [SerializeField] float boostedGroundDrag;

    [Header("Air Variables")]
    [SerializeField] float airAcceleration;
    [SerializeField] float maxAirSpeed;
    [SerializeField] float airDrag;

    [Header("Boosted Air")]
    [SerializeField] float boostedAirAcceleration;
    [SerializeField] float boostedMaxAirSpeed;
    [SerializeField] float boostedAirDrag;

    [Header("Jump Variables")]
    [SerializeField] float jumpForce;
    [SerializeField] float gravityAcceleration;

    [Header("Boosted Jump")]
    [SerializeField] float boostedJumpForce;
    [SerializeField] float boostedGravityAcceleration;

    [Header("Other Variables")]
    [Range(0.0f, 1f)]
    [SerializeField] float rotationSpeed;

    bool readyToJump = true;
    bool grounded;

    bool isMoving = false;
    Vector3 targetDirection;

    void Start()
    {
        playerPositionCheck = GetComponent<PlayerPositionCheck>();
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponent<MovementModification>();
    }

    private void FixedUpdate() {
        if (isMoving) {
            MoveUpdate();
            RotationUpdate();
        }

        //Assinging drag
        if (grounded) {
            rb.drag = groundDrag;
        }
        else {
            rb.drag = airDrag;
            rb.velocity += Vector3.down * Mathf.Lerp(gravityAcceleration, boostedGravityAcceleration, movementModification.boostForAll);
        }
    }
    private void Update()
    {
        grounded = playerPositionCheck.CheckOnGround();
        if(!readyToJump && grounded) {
            ResetJump();
        }
    }

    #region // Move Methods
    public void MoveInput(Vector3 inputDirection)
    {
        isMoving = true;
        targetDirection = new Vector3(inputDirection.x, 0, inputDirection.z).normalized;
    }

    public void NoMoveInput() {
        isMoving = false;
    }
    private void MoveUpdate()
    {
        Vector3 addedVelocity;
        Vector3 maxVelocity;
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // Assigning variables if player is on the ground
        if (grounded) {
            addedVelocity = targetDirection.normalized * Mathf.Lerp(groundAcceleration, boostedGroundAcceleration, movementModification.boostForAll);
            maxVelocity = targetDirection.normalized * Mathf.Lerp(maxGroundSpeed, boostedMaxGroundSpeed, movementModification.boostForAll);
        }
        else {
            addedVelocity = targetDirection.normalized * Mathf.Lerp(airAcceleration, boostedAirAcceleration, movementModification.boostForAll);
            maxVelocity = targetDirection.normalized * Mathf.Lerp(maxAirSpeed, boostedMaxAirSpeed, movementModification.boostForAll);
        }

        // Applying horizontal movement and limiting speed based on max velocity
        float alignment = Vector3.Dot(horizontalVelocity/maxVelocity.magnitude, maxVelocity/maxVelocity.magnitude);
        Physics.Raycast(rb.position, addedVelocity);
        if (alignment < 1 && !playerPositionCheck.CheckColldingWithTerrain(addedVelocity)) {
            rb.AddForce(addedVelocity, ForceMode.VelocityChange);
        }
    }
    #endregion


    #region // Jump Methods
    public void JumpInput() {
        readyToJump = false;
        grounded = false;
        Jump(1);
    }

    public void Jump(float multiplier) {
        float netJumpForce = Mathf.Lerp(jumpForce, boostedJumpForce, movementModification.boostForAll);
        rb.AddForce(transform.up * netJumpForce * multiplier, ForceMode.VelocityChange);
    }
    public void ResetJump() {
        readyToJump = true;
    }
    public bool CanPerformJump() {
        return readyToJump && grounded;
    }
    #endregion

    private void RotationUpdate() {
        transform.forward = Vector3.Lerp(transform.forward, targetDirection.normalized, rotationSpeed);
    }

    public override void EndAction() {
        return;
    }
}
