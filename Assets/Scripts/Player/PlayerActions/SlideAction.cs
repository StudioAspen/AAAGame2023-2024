using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

public class SlideAction : PlayerAction{
    [SerializeField] float slideSpeed;
    [SerializeField] float boostedSlideSpeed;
    float currentSlideSpeed;
    
    private Rigidbody rb;
    private PathCreator pathCreator;
    MovementModification movementModification;

    private GameObject swordObject;
    private EndOfPathInstruction end;
    private bool sliding = false;

    private Vector3 playerOffset;
    //private Vector3 swordOffset;
    private float dstTravelled = 0f;
    
    private void Start() {
        rb = GetComponent<Rigidbody>();
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
        //swordOffset = swordObject.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
    }
    
    private void UpdateSliding() {
        Debug.Log(dstTravelled);
        Debug.Log(currentSlideSpeed);
        dstTravelled += currentSlideSpeed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + playerOffset;
        //swordObject.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + swordOffset;
        //swordObject.transform.up = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

        if (dstTravelled > pathCreator.path.length) {
            EndAction();
        }
    }

    public override void EndAction() {
        dstTravelled = 0f;
        sliding = false;
        rb.useGravity = true;
        OnEndAction.Invoke();
    }
}
