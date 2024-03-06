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
    [SerializeField] float groundAcceleration; // How fast the player accelerates to max speed on the ground
    [SerializeField] float maxGroundSpeed; // Max ground speed
    [SerializeField] float groundFriction; // How much the player slows down when on the ground

    [Header("Boosted Ground")]
    [SerializeField] float boostedGroundAcceleration;
    [SerializeField] float boostedMaxGroundSpeed;
    [SerializeField] float boostedGroundFriction;

    [Header("Air Variables")]
    [SerializeField] float airAcceleration; // How fast the player accelerate to max speed in the air
    [SerializeField] float maxAirSpeed; // Max air speed
    [SerializeField] float airDrag; // How much the player slows down in the air

    [Header("Boosted Air")]
    [SerializeField] float boostedAirAcceleration;
    [SerializeField] float boostedMaxAirSpeed;
    [SerializeField] float boostedAirDrag;

    [Header("Jump Variables")]
    [SerializeField] float jumpForce; // How high the player jumps ON PLAYER INPUT JUMP
    [SerializeField] float gravityAcceleration; // How fast the player accelerates due to gravity
    [SerializeField] float maxFallSpeed; // The max fall speed DUE TO GRAVITY, things like the downward stab can make you fall faster than this

    [Header("Boosted Jump")]
    [SerializeField] float boostedJumpForce;
    [SerializeField] float boostedGravityAcceleration;
    [SerializeField] float boostedMaxFallSpeed;

    [Header("Other Variables")]
    [Range(0.0f, 1f)]
    [SerializeField] float rotationSpeed; // How fast the player object turns to the direction inputted by player

    bool canAirJump = false;
    bool grounded;
    bool isMoving = false;
    Vector3 targetDirection;

    void Start() {
        // Getting references
        movementModification = GetComponentInChildren<MovementModification>();
        playerPositionCheck = GetComponentInChildren<PlayerPositionCheck>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (isMoving) {
            MoveUpdate();
            RotationUpdate();
        }


        RaycastHit hit = playerPositionCheck.CheckColldingWithTerrain(transform.forward);
        float collisionAlignment = Vector3.Dot(hit.normal, transform.forward);

        Debug.DrawLine(transform.position, hit.point, Color.blue, 1f);

        //Implemented physics
        if (grounded) {
            if (rb.velocity.magnitude > 0) {
                Vector3 frictionChange = rb.velocity.normalized * movementModification.GetBoost(groundFriction, boostedGroundFriction, false);
                if(frictionChange.magnitude < rb.velocity.magnitude) {
                    rb.velocity -= frictionChange;
                }
                else {
                    rb.velocity = Vector3.zero;
                }
            }
        }
        else {
            rb.drag = airDrag;
            if (rb.velocity.magnitude < movementModification.GetBoost(maxFallSpeed, boostedMaxFallSpeed, false)) {
                rb.velocity += Vector3.down * movementModification.GetBoost(gravityAcceleration, boostedGravityAcceleration, false);
            }
        }
    }
    private void Update()
    {
        grounded = playerPositionCheck.CheckOnGround();
        if(grounded) {
            canAirJump = false;
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
            addedVelocity = targetDirection.normalized * movementModification.GetBoost(groundAcceleration, boostedGroundAcceleration, true);
            maxVelocity = targetDirection.normalized * movementModification.GetBoost(maxGroundSpeed, boostedMaxGroundSpeed, true);
        }
        else {
            addedVelocity = targetDirection.normalized * movementModification.GetBoost(airAcceleration, boostedAirAcceleration, true);
            maxVelocity = targetDirection.normalized * movementModification.GetBoost(maxAirSpeed, boostedMaxAirSpeed, true);
        }

        // Applying horizontal movement and limiting speed based on max velocity
        float alignment = Vector3.Dot(horizontalVelocity/maxVelocity.magnitude, maxVelocity/maxVelocity.magnitude);
        Physics.Raycast(rb.position, addedVelocity);


        //RaycastHit hit = playerPositionCheck.CheckColldingWithTerrain(addedVelocity);
        //float collisionAlignment = Vector3.Dot(hit.normal, addedVelocity);

        //Debug.DrawLine(transform.position, hit.point, Color.blue, 1f);

        //Debug.Log(collisionAlignment);
        if (alignment < 1) {
            rb.velocity += addedVelocity;
        }
    }
    #endregion


    #region // Jump Methods
    public void JumpInput() {
        if(grounded || canAirJump) {
            if(canAirJump) {
                canAirJump = false;
            }
            grounded = false;
            Jump(Mathf.Lerp(jumpForce, boostedJumpForce, movementModification.boostForAll));
        }
    }

    public void Jump(float _jumpForce) {
        if(rb.velocity.y < 0) {
            rb.velocity = transform.up * _jumpForce;
        }
        else {
            rb.velocity += transform.up * _jumpForce;
        }
    }
    public void GiveAirJump() {
        canAirJump = true;
    }
    #endregion

    private void RotationUpdate() {
        transform.forward = Vector3.Lerp(transform.forward, targetDirection.normalized, rotationSpeed);
    }

    public override void EndAction() {
        return;
    }

    public float GetMaxRunningSpeed() {
        if (grounded) {
            return movementModification.GetBoost(maxGroundSpeed, boostedMaxGroundSpeed, true);
        }
        return movementModification.GetBoost(maxAirSpeed, boostedMaxAirSpeed, true);

    }
}
