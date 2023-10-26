using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlashAndSlide : MonoBehaviour
{
    public float slideSpeed;

    UnityEvent OnSlashStart;
    UnityEvent OnSlashEnd;

    private Rigidbody rb;
    private bool sliding = false;
    private Vector3 dashDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (sliding)
        {
            rb.velocity = slideSpeed * Time.fixedDeltaTime * dashDir.normalized;
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
        if (other.TryGetComponent<Slashable>(out Slashable slashable))
        {
            sliding = true;
            dashDir = slashable.dashDir;

            // Pause slash animation
        }
    }

    public void SlashContactEnd(GameObject other)
    {
        if (other.TryGetComponent<Slashable>(out Slashable slashable))
        {
            sliding = false;
            if (OnSlashEnd != null)
            {
                OnSlashEnd.Invoke();
            }

            // Continue slash animation
        }
    }

    public void InterruptSlash()
    {

    }

}
