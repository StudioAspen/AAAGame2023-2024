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
    public float bloodGained;

    UnityEvent OnSlashStart;
    UnityEvent OnSlashEnd;

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
    }

    private void Update()
    {
        if (sliding)
        {
            dstTravelled += slideSpeed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + playerOffset;
            sword.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end) + swordOffset;
            sword.transform.up = pathCreator.path.GetNormalAtDistance(dstTravelled, end);

            if (dstTravelled > pathCreator.path.length)
            {
                dstTravelled = 0f;
                sliding = false;
                rb.useGravity = true;

                if (TryGetComponent<BloodThirst>(out BloodThirst bt))
                {
                    bt.GainBlood(bloodGained);
                }

                if (TryGetComponent<PlayerMovement>(out PlayerMovement pm))
                {
                    pm.Jump();
                    pm.ResetJump();
                }

                if (TryGetComponent<DashMovement>(out DashMovement dm)) {
                    dm.ResetDash();
                }
            }
        }
    }

    public void StartSlash()
    {
        if (OnSlashStart != null)
        {
            OnSlashStart.Invoke();
        }

        // Start slash animation
    }

    public void SlashContact(GameObject other, Vector3 contactPoint)
    {
        if (other.TryGetComponent<Slashable>(out Slashable slashable) && 
            other.TryGetComponent<PathCreator>(out PathCreator pc))
        {
            sliding = true;
            pathCreator = pc;
            rb.useGravity = false;
            dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(contactPoint);
            playerOffset = transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);
            swordOffset = sword.transform.position - pathCreator.path.GetPointAtDistance(dstTravelled, end);

            // Pause slash animation
        }
    }

    public void InterruptSlash()
    {

    }

}
