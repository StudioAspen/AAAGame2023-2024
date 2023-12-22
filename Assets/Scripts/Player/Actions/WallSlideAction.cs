using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

public class WallSlideAction : MonoBehaviour {
    [Header("References")]
    [SerializeField] Rigidbody rb;

    [Header("Events")]
    public UnityEvent OnSlideEnd = new UnityEvent();

    [Header("State")]
    public bool isSliding = false;

    float slideSpeed;
    GameObject swordObject;

    private PathCreator pathCreator;
    private EndOfPathInstruction end;
    private float dstTravelled = 0f;

    private Vector3 playerOffset;
    private Vector3 swordOffset;

    private void Start() {
        end = EndOfPathInstruction.Stop;
    }

    private void Update() {
        if (isSliding) {
            UpdateSliding();
        }
    }

    public void StartSlide(PathCreator pc, GameObject other, GameObject _swordObject, float _slideSpeed) {
        // Slide Initalization
        isSliding = true;
        slideSpeed = _slideSpeed;
        swordObject = _swordObject;
        Vector3 contactPoint = other.GetComponent<Collider>().ClosestPoint(swordObject.transform.position);
        PathCreator pathCreator = pc;
        rb.useGravity = false;
        dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);
        playerOffset = rb.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
        swordOffset = swordObject.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
    }

    public void InterruptWallSlide() {
        EndSlide();
    }

    private void UpdateSliding() {
        dstTravelled += slideSpeed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + playerOffset;
        swordObject.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + swordOffset;
        swordObject.transform.up = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

        if (dstTravelled > pathCreator.path.length) {
            EndSlide();
        }
    }

    private void EndSlide() {
        dstTravelled = 0f;
        isSliding = false;
        rb.useGravity = true;
        OnSlideEnd.Invoke();
    }
}
