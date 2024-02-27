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
    [SerializeField] float groundFriction;

    [Header("Boosted Ground")]
    [SerializeField] float boostedGroundAcceleration;
    [SerializeField] float boostedMaxGroundSpeed;
    [SerializeField] float boostedGroundFriction;

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
    [SerializeField] float maxFallSpeed;

    [Header("Boosted Jump")]
    [SerializeField] float boostedJumpForce;
    [SerializeField] float boostedGravityAcceleration;
    [SerializeField] float boostedMaxFallSpeed;

    [Header("Other Variables")]
    [Range(0.0f, 1f)]
    [SerializeField] float rotationSpeed;

    bool canAirJump = false;
    [SerializeField] bool grounded;

    [SerializeField] bool isMoving = false;
    Vector3 targetDirection;

    void Start() {
        // Getting references
        movementModification = GetComponentInChildren<MovementModification>();
        playerPositionCheck = GetComponentInChildren<PlayerPositionCheck>();
        rb = GetComponent<Rigidbody>();

        // Setting physics variables
        rb.drag = airDrag;
    }

    private void FixedUpdate() {
        if (isMoving) {
            MoveUpdate();
            RotationUpdate();
        }

        //Implemented physics
        if (grounded) {
            if (rb.velocity.magnitude > 0) {
                Vector3 frictionChange = rb.velocity.normalized * movementModification.GetBoost(groundFriction, boostedGroundFriction, false) * Time.fixedDeltaTime;
                if(frictionChange.magnitude < rb.velocity.magnitude) {
                    rb.velocity -= frictionChange;
                }
                else {
                    rb.velocity = Vector3.zero;
                }
            }
        }
        else {
            if(rb.velocity.magnitude < movementModification.GetBoost(maxFallSpeed, boostedMaxFallSpeed, false))
            rb.velocity += Vector3.down * movementModification.GetBoost(gravityAcceleration, boostedGravityAcceleration, false) * Time.fixedDeltaTime;
        }
    }
    private void Update()
    {
        grounded = playerPositionCheck.CheckOnGround();
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
        if (alignment < 1 && !playerPositionCheck.CheckColldingWithTerrain(addedVelocity)) {
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
        rb.velocity = transform.up * _jumpForce;
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
