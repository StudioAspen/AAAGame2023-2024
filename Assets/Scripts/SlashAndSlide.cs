using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SlashAndSlide : MonoBehaviour
{
    public Transform sword;
    public float slideSpeed;

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

            if (dstTravelled > pathCreator.path.length)
            {
                dstTravelled = 0f;
                sliding = false;
                rb.useGravity = true;
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

    public void SlashContact(GameObject other)
    {
        if (other.TryGetComponent<Slashable>(out Slashable slashable) && 
            other.TryGetComponent<PathCreator>(out PathCreator pc))
        {
            sliding = true;
            pathCreator = pc;
            rb.useGravity = false;
            playerOffset = transform.position - pathCreator.path.GetPointAtDistance(0, end);
            swordOffset = sword.transform.position - pathCreator.path.GetPointAtDistance(0, end);

            // Pause slash animation
        }
    }

    public void InterruptSlash()
    {

    }

}
