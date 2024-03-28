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
    private Collider playerCollider;
    private PathCreator pathCreator;
    MovementModification movementModification;
    PlayerPositionCheck playerPositionCheck;

    private EndOfPathInstruction end;
    private bool sliding = false;

    [Header("References")]
    [SerializeField] GameObject swordObject;
    private Vector3 initialSwordOffset;
    private Vector3 initialPlayerOffset;
    private Quaternion initialPlayerRotation;
    private Vector3 inputDir;
    private Vector3 startVelocity;
    private float initialEnemyBonus;
    private float dstTravelled = 0f;

    private Quaternion initialWallRotation;

    JumpAction jumpAction;
    
    private void Start() {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
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

    public void StartSlide(PathCreator pc, Collider other, float enemyBonus = 0) {
        // Slide Initalization
        sliding = true;
        pathCreator = pc;
        initialEnemyBonus = enemyBonus;

        // Calculating inital speed
        startVelocity = rb.velocity * movementModification.GetBoost(initalSpeedScale, boostedInitalSpeedScale, false);
        currentSlideSpeed = movementModification.GetBoost(slideSpeed, boostedSlideSpeed, true) + initialEnemyBonus;
        currentSlideSpeed += startVelocity.magnitude;

        // Limiting Speed
        float currentSpeedLimit = movementModification.GetBoost(speedLimit, boostsedSpeedLimit, false);
        currentSlideSpeed = Mathf.Min(currentSpeedLimit, currentSlideSpeed);

        // Initalizing offset
        Vector3 contactPoint = other.ClosestPoint(transform.position);
        dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);

        // saving initalizing values for rotations
        initialWallRotation = pathCreator.transform.rotation;
        initialPlayerOffset = transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
        initialPlayerRotation = transform.rotation;
        initialSwordOffset = swordObject.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);

        // On start action event
        OnStartAction.Invoke();
    }
    
    private void UpdateSliding() {
        // Initalizing offsets by rotation transformation
        Quaternion rotationTransformation = pathCreator.transform.rotation * Quaternion.Inverse(initialWallRotation);
        Vector3 playerOffset = rotationTransformation * initialPlayerOffset;
        Vector3 swordOffset = rotationTransformation * initialSwordOffset;

        Vector3 pathPoint = pathCreator.path.GetPointAtDistance(dstTravelled, end);
        Vector3 pathNormal = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

        // Applying speed
        dstTravelled += (currentSlideSpeed * Time.fixedDeltaTime) / pathCreator.path.CalculatePathWorldLength();
        transform.position = pathPoint + playerOffset;
        transform.rotation = rotationTransformation*initialPlayerRotation;
        swordObject.transform.position = pathPoint + swordOffset;
        swordObject.transform.up = pathNormal;

        // Ending the dash with movement abilities
        if (dstTravelled > pathCreator.path.length) {
            // Calculating jump variables and limits
            float currentJumpLimit = movementModification.GetBoost(jumpForceLimit, boostedJumpForceLimit, false);
            float currentJumpForce = movementModification.GetBoost(jumpForce, boostedJumpForce, true) + startVelocity.magnitude + initialEnemyBonus;
            
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
        Vector3 addedVelocity = inputDir.normalized * (movementModification.GetBoost(exitOffsetSpeed, boostedExitOffsetSpeed, true) + startVelocity.magnitude + initialEnemyBonus);
        addedVelocity = playerPositionCheck.CorrectVelocityCollision(addedVelocity);

        rb.velocity += addedVelocity;
    }
    public override void EndAction() {
        transform.SetParent(null);
        dstTravelled = 0f;
        sliding = false;
        initialEnemyBonus = 0;

        OnEndAction.Invoke();
    }
}