using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

public class SlideMovement {
    private Rigidbody rb;
    private Transform transform;
    private PathCreator pathCreator;
    private EndOfPathInstruction end;
    private bool sliding = false;
    private GameObject swordObject;

    private Vector3 playerOffset;
    //private Vector3 swordOffset;

    private float dstTravelled = 0f;
    private float slideSpeed;

    UnityEvent OnEndSlide = new UnityEvent();

    SlideMovement(Rigidbody _rb, Transform _transform) {
        end = EndOfPathInstruction.Stop;
        rb = _rb;
        transform = _transform;
    }


    public void StartSlide(PathCreator pc, Collider other, float _slideSpeed) {
        // Slide Initalization
        sliding = true;
        pathCreator = pc;
        slideSpeed = _slideSpeed;

        Vector3 contactPoint = other.ClosestPoint(swordObject.transform.position);
        rb.useGravity = false;
        dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);
        playerOffset = transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
        //swordOffset = swordObject.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
    }
    
    private void UpdateSliding() {
        dstTravelled += slideSpeed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + playerOffset;
        //swordObject.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + swordOffset;
        //swordObject.transform.up = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

        if (dstTravelled > pathCreator.path.length) {
            EndSlide();
        }
    }

    private void EndSlide() {
        dstTravelled = 0f;
        sliding = false;
        rb.useGravity = true;
        OnEndSlide.Invoke();
    }

}
