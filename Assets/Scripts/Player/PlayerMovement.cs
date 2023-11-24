using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Components
    PlayerPositionCheck playerPositionCheck;
    Rigidbody rb;
    MovementModification movementModification;

    [Header("Ground Variables")]
    public float groundAcceleration;
    public float maxGroundSpeed;
    public float groundDrag;
    [Header("Boosted Ground")]
    public float boostedGroundAcceleration;
    public float boostedMaxGroundSpeed;
    public float boostedGroundDrag;

    [Header("Air Variables")]
    public float airAcceleration;
    public float maxAirSpeed;
    public float airDrag;
    [Header("Boosted Air")]
    public float boostedAirAcceleration;
    public float boostedMaxAirSpeed;
    public float boostedAirDrag;

    [Header("Jump Variables")]
    public float jumpForce;
    public float gravityAcceleration;
    [Header("Boosted Jump")]
    public float boostedJumpForce;
    public float boostedGravityAcceleration;

    [Header("Other Variables")]
    [Range(0.0f, 1f)]
    public float rotationSpeed;

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
        if(grounded) {
            ResetJump();
        }
    }


    public void JumpFunction()
    {
        if(readyToJump)
        {
            readyToJump = false;
            grounded = false;
            Jump(1);
        }
    }

    public void Move(Vector3 inputDirection)
    {
        if (inputDirection.magnitude > 0)
        {
            isMoving = true;
            targetDirection = new Vector3(inputDirection.x, 0, inputDirection.z).normalized;
        }
        else
        {
            isMoving = false;
        }
    }
    public void Jump(float multiplier) {
        float netJumpForce = Mathf.Lerp(jumpForce, boostedJumpForce, movementModification.boostForAll);
        rb.AddForce(transform.up * netJumpForce * multiplier, ForceMode.VelocityChange);
    }

    public void ResetJump() {
        readyToJump = true;
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

        // Applying horizontal movement
        float alignment = Vector3.Dot(horizontalVelocity/maxVelocity.magnitude, maxVelocity/maxVelocity.magnitude);
        Physics.Raycast(rb.position, addedVelocity);
        if (alignment < 1 && !playerPositionCheck.CheckColldingWithTerrain(addedVelocity)) {
            rb.AddForce(addedVelocity, ForceMode.VelocityChange);
        }
    }

    private void RotationUpdate() {
        transform.forward = Vector3.Lerp(transform.forward, targetDirection.normalized, rotationSpeed);
    }
}
