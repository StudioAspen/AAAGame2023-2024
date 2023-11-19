using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Stab : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject swordObject;

    [Header("Other Variables")]
    [SerializeField] float dashSpeed;
    [SerializeField] float bloodGainAmount; 

    [Header("Events")]
    public UnityEvent OnStabEnd = new UnityEvent();

    // Compoenents
    Rigidbody rb;
    DashMovement dashMovement;
    PlayerMovement playerMovement;
    Collider collider;
    DemonSword sword;

    // Variables
    bool isStabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        rb = GetComponent<Rigidbody>();
        dashMovement = GetComponent<DashMovement>();
        collider = GetComponent<Collider>();
        playerMovement = GetComponent<PlayerMovement>();
        
        // Setting up Demon Sword
        sword = swordObject.GetComponent<DemonSword>();
        sword.OnContact.AddListener(StabContact);
    }


    public void StartStab()
    {
        if (!isStabbing) {
            //Animation Stuff (to be implemented later)
            isStabbing = true;

            // Demon sword variables
            sword.OnEndAction.AddListener(EndOfStabAnimation);
            sword.AttackPosition();
        }
    }

    public void InterruptStab()
    {
        isStabbing = false;
        sword.EndAttackPosition();
    }

    public void StabContact(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Stabable stabable))
        {
            if(isStabbing)
            {
                // Setting proper listeners and variables
                isStabbing = false;
                sword.OnEndAction.RemoveListener(EndOfStabAnimation);

                // Applying Gameplay mechanics
                sword.GetComponent<BloodThirst>().GainBlood(bloodGainAmount, true);

                // Setting up and starting dash
                DashThrough(stabable);
            }
        }
    }
    public void DashThrough(Stabable stabable) {
        dashMovement.OnDashEnd.AddListener(EndOfDash);
        collider.isTrigger = true; // Setting as trigger to prevent collisions
        float dashDuration = (stabable.dashLength / dashSpeed);
        rb.position = stabable.dashStartTransform.position;
        dashMovement.Dash(stabable.dashLength, dashDuration, stabable.dashDir);
    }
    private void EndOfDash() {
        dashMovement.OnDashEnd.RemoveListener(EndOfDash);
        playerMovement.ResetJump();
        dashMovement.ResetDash();
        collider.isTrigger = false; // Re-enable collider
        OnStabEnd.Invoke();
    }
    private void EndOfStabAnimation() {
        isStabbing = false;
        sword.OnEndAction.RemoveListener(EndOfStabAnimation);
        OnStabEnd.Invoke();
    }
}
