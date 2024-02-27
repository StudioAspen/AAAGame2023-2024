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

    [Header("Boosted Variables")]
    [SerializeField] float boostedSlideSpeed;
    [SerializeField] float boostedJumpForce;
    [SerializeField] float boostedExitOffsetSpeed;

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
    private float dstTravelled = 0f;

    BasicMovementAction movementAction;
    
    private void Start() {
        rb = GetComponent<Rigidbody>();
        movementAction = GetComponent<BasicMovementAction>();
        movementModification = GetComponentInChildren<MovementModification>();
        end = EndOfPathInstruction.Stop;
    }

    private void Update() {
        if (sliding) {
            UpdateSliding();
        }
    }

    public void StartSlide(PathCreator pc, Collider other) {
        // Slide Initalization
        sliding = true;
        pathCreator = pc;

        currentSlideSpeed = movementModification.GetBoost(slideSpeed, boostedSlideSpeed, true);
        Vector3 contactPoint = other.ClosestPoint(transform.position);
        rb.useGravity = false;
        dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);
        playerOffset = transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
        swordOffset = swordObject.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
    }
    
    private void UpdateSliding() {
        dstTravelled += currentSlideSpeed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + playerOffset;
        swordObject.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + swordOffset;
        swordObject.transform.up = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

        if (dstTravelled > pathCreator.path.length) {
            EndAction();
        }
    }

    public void SlideInput(Vector3 direction) {
        inputDir = direction;
    }

    public override void EndAction() {
        dstTravelled = 0f;
        sliding = false;
        rb.useGravity = true;
        movementAction.Jump(movementModification.GetBoost(jumpForce, boostedJumpForce, true));
        rb.velocity += inputDir.normalized * movementModification.GetBoost(exitOffsetSpeed, boostedExitOffsetSpeed, true);
        OnEndAction.Invoke();
    }
}
