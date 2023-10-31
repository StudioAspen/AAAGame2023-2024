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
    private float dstTravelled;

    private bool sliding = false;

    private void Start()
    {
        end = EndOfPathInstruction.Stop;
    }

    private void Update()
    {
        if (sliding)
        {
            dstTravelled += slideSpeed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
            sword.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
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

            // Pause slash animation
        }
    }

    public void InterruptSlash()
    {

    }

}
