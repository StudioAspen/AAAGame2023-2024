using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

public class SlideAction : PlayerAction{
    [Header("Regular Variables")]
    [SerializeField] float slideSpeed; // How fast you are moving while sliding
    [SerializeField] float jumpForce; // Jump force at the end fo the slide
    [SerializeField] float exitOffsetSpeed; // the force applied at the end of the slide HORIZONTALLY based on the player inputs
    [SerializeField] float initalSpeedScale;  // How much the player impacts the speed, measured in percent (i.e. value of 0.1 == 10% of player speed is factored)
    [SerializeField] float speedLimit; // The max speed AFTER inital velocity + speed + bonus speed CALCULATION (so this limit applies for both the exit speed and the action itself) 
    [SerializeField] float jumpForceLimit; // Same as the speed limit but applied to the jump force

    [Header("Boosted Variables")]
    [SerializeField] float boostedSlideSpeed;
    [SerializeField] float boostedJumpForce;
    [SerializeField] float boostedExitOffsetSpeed;
    [SerializeField] float boostedInitalSpeedScale;
    [SerializeField] float boostsedSpeedLimit;
    [SerializeField] float boostedJumpForceLimit;
    
    float currentSlideSpeed;
    
    private Rigidbody rb;
    private PathCreator pathCreator;
    MovementModification movementModification;
    PlayerPositionCheck playerPositionCheck;

    private EndOfPathInstruction end;
    private bool sliding = false;

    [Header("References")]
    [SerializeField] GameObject swordObject;
    private Vector3 swordOffset;
    private Vector3 playerOffset;
    private Vector3 inputDir;
    private Vector3 startVelocity;
    private float dstTravelled = 0f;

    JumpAction jumpAction;
    
    private void Start() {
        rb = GetComponent<Rigidbody>();
        jumpAction = GetComponent<JumpAction>();
        movementModification = GetComponentInChildren<MovementModification>();
        playerPositionCheck = GetComponentInChildren<PlayerPositionCheck>();
        end = EndOfPathInstruction.Stop;
    }

    private void FixedUpdate() {
        if (sliding) {
            UpdateSliding();
        }
    }

    public void StartSlide(PathCreator pc, Collider other) {
        // Slide Initalization
        sliding = true;
        pathCreator = pc;

        // Calculating inital speed
        startVelocity = rb.velocity * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);
        currentSlideSpeed = movementModification.GetBoost(slideSpeed, boostedSlideSpeed, true);
        currentSlideSpeed += startVelocity.magnitude;

        // Limiting Speed
        float currentSpeedLimit = movementModification.GetBoost(speedLimit, boostsedSpeedLimit, false);
        currentSlideSpeed = Mathf.Min(currentSpeedLimit, currentSlideSpeed);

        // Initalizing offset
        Vector3 contactPoint = other.ClosestPoint(transform.position);
        dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);
        playerOffset = transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
        swordOffset = swordObject.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);

        OnStartAction.Invoke();
    }
    
    private void UpdateSliding() {
        // Applying speed
        dstTravelled += currentSlideSpeed * Time.fixedDeltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + playerOffset;
        swordObject.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + swordOffset;
        swordObject.transform.up = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

        // Ending the dash with movement abilities
        if (dstTravelled > pathCreator.path.length) {
            // Calculating jump variables and limits
            float currentJumpLimit = movementModification.GetBoost(jumpForceLimit, boostedJumpForceLimit, false);
            float currentJumpForce = movementModification.GetBoost(jumpForce, boostedJumpForce, true) + startVelocity.magnitude;
            
            // Applying limits
            jumpAction.Jump(Mathf.Min(currentJumpLimit, currentJumpForce));
            ApplyHorizontalOffset();
            EndAction();
        }
    }

    public void SlideInput(Vector3 direction) {
        inputDir = direction.normalized;
    }
    public void ApplyHorizontalOffset() {
        Vector3 addedVelocity = inputDir.normalized * (movementModification.GetBoost(exitOffsetSpeed, boostedExitOffsetSpeed, true) + startVelocity.magnitude);
        addedVelocity = playerPositionCheck.CorrectVelocityCollision(addedVelocity);

        rb.velocity += addedVelocity;
    }
    public override void EndAction() {
        dstTravelled = 0f;
        sliding = false;

        OnEndAction.Invoke();
    }
}
