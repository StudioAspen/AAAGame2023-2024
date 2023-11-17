using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Components
    Rigidbody rb;
    GroundCheck groundCheck;

    [Header("Ground Variables")]
    public float groundAcceleration;
    public float maxGroundSpeed;
    public float groundDrag;

    [Header("Air Variables")]
    public float airAcceleration;
    public float maxAirSpeed;
    public float airDrag;

    [Header("Jump Variables")]
    public float jumpForce;
    public float gravityAcceleration;

    [Header("Other Variables")]
    [Range(0.0f, 1f)]
    public float rotationSpeed;

    bool readyToJump = true;
    bool grounded;

    bool isMoving = false;
    Vector3 targetDirection;

    void Start()
    {
       rb = GetComponent<Rigidbody>();
        groundCheck = GetComponent<GroundCheck>();
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
            rb.velocity += Vector3.down * gravityAcceleration;
        }
    }
    private void Update()
    {
        grounded = groundCheck.CheckOnGround();

        if(grounded) {
            ResetJump();
        }
    }


    public void JumpFunction()
    {
        if(readyToJump)
        {
            readyToJump = false;
            Jump();
        }

    }

    public void Move(Vector3 inputDirection)
    {
        if (inputDirection.magnitude > 0.1f)
        {
            isMoving = true;
            targetDirection = new Vector3(inputDirection.x, 0, inputDirection.z).normalized;
        }
        else
        {
            isMoving = false;
        }
    }
    public void Jump() {
        //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
    }

    public void ResetJump() {
        readyToJump = true;
    }
    private void MoveUpdate()
    {
        float currentMaxSpeed;
        float alignment;
        Vector3 addedVelocity;

        // Assigning variables if player is on the ground
        if (grounded) {
            addedVelocity = targetDirection.normalized * groundAcceleration;
            alignment = Vector3.Dot(rb.velocity, addedVelocity);
            currentMaxSpeed = maxGroundSpeed;
        }
        else {
            addedVelocity = targetDirection.normalized * airAcceleration;
            alignment = Vector3.Dot(rb.velocity, addedVelocity);
            currentMaxSpeed = maxAirSpeed;
        }

        // Applying horizontal movement
        if (rb.velocity.magnitude < currentMaxSpeed || alignment < 0) {
            rb.AddForce(addedVelocity, ForceMode.VelocityChange);
        }
    }
    private void RotationUpdate() {
        transform.forward = Vector3.Lerp(transform.forward, targetDirection.normalized, rotationSpeed);
    }
}
