using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAndSlide : MonoBehaviour
{
    public GameObject sword;
    public float climbSpeed;

    private enum State {idling, slashing, climbing};
    private State state;

    private float slashCurrTime = 0f;
    private float slashMaxTime = 1f;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    private Rigidbody rb;

    private void Start()
    {
        state = State.idling;

        startRotation = Quaternion.Euler(90f, 0f, 0f);
        targetRotation = Quaternion.Euler(-90f, 0f, 0f);

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Controls();
        ManageState();
    }

    private void FixedUpdate()
    {
        if (state == State.climbing)
        {
            rb.velocity = climbSpeed * Time.fixedDeltaTime * Vector3.up;
        }
    }

    private void Controls()
    {
        if (state == State.idling && Input.GetMouseButtonDown(0))
        {
            slashCurrTime = 0f;
            state = State.slashing;
        }
    }

    private void ManageState()
    {
        if (state == State.slashing)
        {
            slashCurrTime = Mathf.Clamp(slashCurrTime + Time.deltaTime, 0f, slashMaxTime);
            sword.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, slashCurrTime / slashMaxTime);

            if (slashCurrTime > slashMaxTime - 0.01f)
            {
                state = State.idling;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slashable"))
        {
            state = State.climbing;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slashable"))
        {
            state = State.slashing;
        }
    }
}
