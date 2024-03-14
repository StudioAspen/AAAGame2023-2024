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
    

    [Header("Boosted Variables")]
    [SerializeField] float boostedSlideSpeed;
    [SerializeField] float boostedJumpForce;
    [SerializeField] float boostedExitOffsetSpeed;
    [SerializeField] float boostedInitialSpeedScale;
    
    float currentSlideSpeed;
    
    private Rigidbody rb;
    private PathCreator pathCreator;
    MovementModification movementModification;

    private EndOfPathInstruction end;
    private bool sliding = false;

    [Header("References")]
    [SerializeField] GameObject swordObject;
    private Vector3 swordOffset;
    private Vector3 playerOffset;
    private Vector3 inputDir;
    private Vector3 startVelocity;
    private float dstTravelled = 0f;

    BasicMovementAction movementAction;
    
    private void Start() {
        rb = GetComponent<Rigidbody>();
        movementAction = GetComponent<BasicMovementAction>();
        movementModification = GetComponentInChildren<MovementModification>();
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
        startVelocity = rb.velocity * movementModification.GetBoost(initalSpeedScale, boostedInitialSpeedScale, false);

        currentSlideSpeed = movementModification.GetBoost(slideSpeed, boostedSlideSpeed, true);
        currentSlideSpeed += startVelocity.magnitude;

        Vector3 contactPoint = other.ClosestPoint(transform.position);
        rb.useGravity = false;
        dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);

        transform.SetParent(pathCreator.transform, true);

        playerOffset = pathCreator.transform.InverseTransformPoint(transform.position);
        swordOffset = pathCreator.transform.InverseTransformPoint(swordObject.transform.position);
    }
    
    private void UpdateSliding() {
        dstTravelled += currentSlideSpeed * Time.fixedDeltaTime;
        Vector3 pathPoint = pathCreator.path.GetPointAtDistance(dstTravelled, end);
        Vector3 pathNormal = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

        transform.localPosition = pathCreator.transform.InverseTransformPoint(pathPoint) + playerOffset;
        swordObject.transform.localPosition = pathCreator.transform.InverseTransformPoint(pathPoint) + swordOffset;
        swordObject.transform.up = pathNormal;

        if (dstTravelled > pathCreator.path.length) {
            EndAction();
        }
    }

    public void SlideInput(Vector3 direction) {
        inputDir = direction;
    }

    public override void EndAction() {
        transform.SetParent(null);
        dstTravelled = 0f;
        sliding = false;
        rb.useGravity = true;
        movementAction.Jump(movementModification.GetBoost(jumpForce, boostedJumpForce, true) + startVelocity.magnitude);
        rb.velocity += inputDir.normalized * (movementModification.GetBoost(exitOffsetSpeed, boostedExitOffsetSpeed, true) + startVelocity.magnitude);
        OnEndAction.Invoke();
    }
}