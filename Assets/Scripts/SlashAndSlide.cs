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
            rb.velocity = slideSpeed * Time.fixedDeltaTime * dashDir;
        }
    }

    public void StartSlash()
    {
        OnSlashStart.Invoke();

        // Start slash animation
    }

    public void SlashContact(GameObject other)
    {
        sliding = true;
        if (other.TryGetComponent<Slashable>(out Slashable slashable))
        {
            dashDir = slashable.dashDir;
        }
    }

    public void InteruptSlash()
    {

    }

}
