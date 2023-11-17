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
    PlayerInput playerInput;
    PlayerMovement playerMovement;
    Collider collider;
    [SerializeField] private GameObject swordObject;
    DemonSword demonSword;

    [Header("Other Variables")]
    [SerializeField] float dashSpeed;
    [SerializeField] float bloodGainAmount; 
    bool isStabbing = false;

    [Header("Events")]
    //Stab Events
    public UnityEvent onStabEnd = new UnityEvent();
    

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        rb = GetComponent<Rigidbody>();
        dashMovement = GetComponent<DashMovement>();
        collider = GetComponent<Collider>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        
        // Setting up Demon Sword
        demonSword = swordObject.GetComponent<DemonSword>();
        demonSword.OnContact.AddListener(StabContact);
    }


    public void StartStab()
    {
        if (!isStabbing) {
            //Animation Stuff (to be implemented later)
            isStabbing = true;

            // Demon sword variables
            demonSword.OnEndAction.AddListener(EndOfStabAnimation);
            demonSword.AttackPosition();
        }
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
            if(isStabbing)
            {
                // Setting proper listeners and variables
                playerInput.DisableInput();
                isStabbing = false;
                dashMovement.OnDashEnd.AddListener(EndOfDash);
                demonSword.OnEndAction.RemoveListener(EndOfStabAnimation);

                // Applying Gameplay mechanics
                playerMovement.ResetJump();
                dashMovement.ResetDash();
                demonSword.GetComponent<BloodThirst>().GainBlood(bloodGainAmount, true);
                
                // Setting up and starting dash
                collider.isTrigger = true; // Setting as trigger to prevent collisions
                float dashDuration = (stabable.dashLength / dashSpeed);
                rb.position = stabable.dashStartTransform.position;
                dashMovement.Dash(stabable.dashLength, dashDuration, stabable.dashDir);
            }
        }
    }
    private void EndOfDash() {
        dashMovement.OnDashEnd.RemoveListener(EndOfDash);
        collider.isTrigger = false; // Re-enable collider
        playerInput.EnableInput();
        onStabEnd.Invoke();
    }
    private void EndOfStabAnimation() {
        isStabbing = false;
        demonSword.OnEndAction.RemoveListener(EndOfStabAnimation);
        onStabEnd.Invoke();
    }
}
