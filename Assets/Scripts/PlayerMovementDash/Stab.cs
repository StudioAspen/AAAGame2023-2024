using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Stab : MonoBehaviour
{
    //Compoenents
    Rigidbody rb;
    DashMovement dashMovement;
    PlayerMovement PlayerMovement;
    Collider collider;
    [SerializeField] private GameObject swordObject;
    DemonSword demonSword;

    //Values
    [SerializeField] private float dashSpeed;
    bool isStabbing = false;
    bool isDashing = false;

    //Stab Events
    public UnityEvent onStabStart = new UnityEvent();
    public UnityEvent onStabEnd = new UnityEvent();
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashMovement = GetComponent<DashMovement>();
        collider = GetComponent<Collider>();
        demonSword = swordObject.GetComponent<DemonSword>();
        demonSword.OnContact.AddListener(StabContact);
    }

    // Update is called once per frame
    //NOTE: Probably better to use an animator, will figure out later
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click and not already moving.
        {
            StartStab();
        }
    }

    void StartStab()
    {
        //Animation Stuff (to be implemented later)
        isStabbing = true;
        demonSword.AttackPosition();
        onStabStart.Invoke();
    }

    public void InterruptStab()
    {
        isStabbing = false;
    }

    void StabContact(Collider other)
    {
        Stabable stabable;
        if(other.gameObject.TryGetComponent<Stabable>(out stabable))
        {
            if(isStabbing && !isDashing)
            {
                isDashing = true;
                collider.isTrigger = true;
                dashMovement.OnDashEnd.AddListener(EndOfDash);
                //Dashing Movement
                float dashDuration = 1 / (dashSpeed / stabable.dashLength);
                rb.position = stabable.dashStartTransform.position;
                dashMovement.Dash(stabable.dashLength, dashDuration, stabable.dashDir);
            }
        }
    }
    public void EndOfDash() {
        dashMovement.OnDashEnd.RemoveListener(EndOfDash);
        isDashing = false;
        collider.isTrigger = false;
        EndStab();
    }
    private void EndStab()
    {
        isStabbing = false;
        onStabEnd.Invoke();
    }
}
