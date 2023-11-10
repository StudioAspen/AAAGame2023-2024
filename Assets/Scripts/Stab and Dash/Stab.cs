using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Stab : MonoBehaviour
{
    //Compoenents
    Rigidbody rb;
    [SerializeField] private GameObject swordObject;
    private DemonSword demonSword;

    //Values
    [SerializeField] private float dashSpeed;
    private bool isStabbing = false;

    //Stab Events
    public UnityEvent onStabStart;
    public UnityEvent onStabEnd;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        Debug.Log("Stab contact");
        if(other.gameObject.TryGetComponent<Stabable>(out stabable))
        {
            if(isStabbing)
            {
                //Dashing Movement
                Debug.Log("perform dash");
            }
        }
    }
    private void EndStab()
    {
        isStabbing = false;
        onStabEnd.Invoke();
    }
}
