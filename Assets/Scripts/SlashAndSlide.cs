using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;
using UnityEngine.UIElements;

public class SlashAndSlide : MonoBehaviour
{
    public Transform sword;
    public float slideSpeed;
    public float boostedSlideSpeed;
    public float bloodGained;

    public UnityEvent OnSlashStart = new UnityEvent();
    public UnityEvent OnSlashEnd = new UnityEvent();

    public UnityEvent OnSlideStart = new UnityEvent();
    public UnityEvent OnSlideEnd = new UnityEvent();

    private MovementModification movementModification;

    private PathCreator pathCreator;
    private EndOfPathInstruction end;
    private float dstTravelled = 0f;

    private Rigidbody rb;
    private bool sliding = false;
    private Vector3 playerOffset;
    private Vector3 swordOffset;

    private void Start()
    {
        end = EndOfPathInstruction.Stop;
        rb = GetComponent<Rigidbody>();

        movementModification = GetComponent<MovementModification>();
    }

    private void Update()
    {
        if (sliding)
        {
            UpdateSliding();
        }
    }

    private void UpdateSliding()
    {
        dstTravelled += Mathf.Lerp(slideSpeed, boostedSlideSpeed, movementModification.boostForAll) * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + playerOffset;
        sword.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + swordOffset;
        sword.transform.up = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

        if (dstTravelled > pathCreator.path.length)
        {
            EndSlide();

            if (TryGetComponent<PlayerMovement>(out PlayerMovement pm))
            {
                pm.Jump();
                pm.ResetJump();
            }

            if (TryGetComponent<DashMovement>(out DashMovement dm))
            {
                dm.ResetDash();
            }
        }
    }

    private void StartSlide()
    {
        OnSlideStart.Invoke();
        sliding = true;

        if (TryGetComponent<BloodThirst>(out BloodThirst bt))
        {
            bt.GainBlood(bloodGained);
        }
    }

    private void EndSlide()
    {
        OnSlideEnd.Invoke();
        dstTravelled = 0f;
        sliding = false;
        rb.useGravity = true;
    }

    public void SlashContact(GameObject other, Vector3 contactPoint)
    {
        if (other.TryGetComponent<Slashable>(out Slashable slashable) && 
            other.TryGetComponent<PathCreator>(out PathCreator pc))
        {
            StartSlide();
            pathCreator = pc;
            rb.useGravity = false;
            dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);
            playerOffset = transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
            swordOffset = sword.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);

            // Pause slash animation
        }
    }

    public void InterruptSlide()
    {
        EndSlide();
    }

}
