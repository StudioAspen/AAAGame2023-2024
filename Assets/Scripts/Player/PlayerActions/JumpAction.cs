using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAction : PlayerAction
{
    // References
    Rigidbody rb;
    PlayerPositionCheck positionCheck;
    MovementModification movementModification;

    [Header("Jump Variables")]
    [SerializeField] float jumpForce; // How high the player jumps ON PLAYER INPUT JUMP
    [SerializeField] float gravityAcceleration; // How fast the player accelerates due to gravity
    [SerializeField] float maxFallSpeed; // The max fall speed DUE TO GRAVITY, things like the downward stab can make you fall faster than this

    [SerializeField] float maxTimeForAppliedForce; // How long the force is going to be applied
    [SerializeField] float initalSpeed; // Inital Speed that is applied reguardless of how long pressed
    [SerializeField] float appliedForce; // force applied while holding down button

    [Header("Boosted Jump")]
    [SerializeField] float boostedJumpForce;
    [SerializeField] float boostedGravityAcceleration;
    [SerializeField] float boostedMaxFallSpeed;

    float jumpTimer = 0f;
    bool jumping = false;
    
    private void Start() {
        // Getting references
        rb = GetComponent<Rigidbody>();
        positionCheck = GetComponentInChildren<PlayerPositionCheck>();
        movementModification = GetComponent<MovementModification>();
    }



    public void JumpInputPressed() {
        Jump(initalSpeed);
        jumping = true;
    }

    public void JumpInputHold() {
        jumpTimer += Time.deltaTime;
        if(jumpTimer < maxTimeForAppliedForce && jumping) {
            rb.velocity += Vector3.up * appliedForce;
        }
        else {
            jumping = false;
        }
    }
    public void JumpInputRelease() {
        jumpTimer = 0;
        jumping = false;
    }

    public void Jump(float _jumpForce) {
        if (rb.velocity.y < 0) {
            rb.velocity = transform.up * _jumpForce;
        }
        else {
            rb.velocity += transform.up * _jumpForce;
        }
    }

    public bool CanJump() {
        return positionCheck.CheckOnGround();
    }
    public override void EndAction() {
        throw new System.NotImplementedException();
    }
}
