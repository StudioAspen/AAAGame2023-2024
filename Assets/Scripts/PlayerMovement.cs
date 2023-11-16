using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    private bool isMoving = false;
    private Vector3 moveDirection;
    Rigidbody rb;

    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
        if(isMoving)
        {
            //MoveUpdate();
            //SpeedControl();
        }
    }
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2F, whatIsGround);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }


    public void JumpFunction()
    {
        if(readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    public void Move(Vector3 inputDirection)
    {
        if (inputDirection.magnitude > 0)
        {
            isMoving = true;
            moveDirection = new Vector3(inputDirection.x, 0, inputDirection.z);
            //Applying horizontal movement
            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
            }
        }
        else
        {
            isMoving = false;
        }
    }
    private void MoveUpdate()
    {
        //Applying horizontal movement
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

        }
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        readyToJump = true;
    }

}
