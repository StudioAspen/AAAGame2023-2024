using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Stab : MonoBehaviour {
    [Header("References")]
    [SerializeField] SwordAnimation swordMovement;

    [Header("Other Variables")]
    [SerializeField] float dashSpeed;
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float bloodGainAmount;
    [SerializeField] float attackDuration;

    [Header("Events")]
    public UnityEvent OnStabEnd = new UnityEvent();

    // Movement Compoenents
    DashMovement dashMovement;
    PlayerMovement playerMovement;
    Collider _collider;
    Rigidbody rb;
    MovementModification movementModification;

    // Variables
    bool isStabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        dashMovement = GetComponent<DashMovement>();
        playerMovement = GetComponent<PlayerMovement>();
        _collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponent<MovementModification>();

        // Setting up Demon Sword
        swordMovement.OnContact.AddListener(StabContact);
    }


    public void StartStab()
    {
        if (!isStabbing) {
            //Animation Stuff (to be implemented later)
            isStabbing = true;

            // Demon sword variables
            swordMovement.OnEndAnimation.AddListener(EndOfStabAnimation);
            swordMovement.StartStabAnimation();
        }
    }

    public void InterruptStab()
    {
        isStabbing = false;
        swordMovement.EndAnimation();
    }

    public void StabContact(GameObject other)
    {
        if(other.TryGetComponent(out StabableTerrain stabableTerrain))
        {
            if(isStabbing)
            {
                // Setting proper listeners and variables
                isStabbing = false;
                swordMovement.OnEndAnimation.RemoveListener(EndOfStabAnimation);

                // Setting up and starting dash
                DashThrough(stabableTerrain);
            }
        }
        if(other.TryGetComponent(out StabbedEffect stabbedEffect)) {
            stabbedEffect.TriggerEffect();
        }
    }
    public void DashThrough(StabableTerrain stabableTerrain) {
        GetComponent<BloodThirst>().GainBlood(bloodGainAmount, true);
        dashMovement.OnDashEnd.AddListener(EndOfDash);
        _collider.isTrigger = true; // Setting as trigger to prevent collisions
        float dashDuration = (stabableTerrain.dashLength / Mathf.Lerp(dashSpeed, boostedDashSpeed, movementModification.boostForAll));
        rb.position = stabableTerrain.dashStartTransform.position;
        dashMovement.Dash(stabableTerrain.dashLength, dashDuration, stabableTerrain.dashDir);
    }
    private void EndOfDash() {
        dashMovement.OnDashEnd.RemoveListener(EndOfDash);
        playerMovement.ResetJump();
        dashMovement.ResetDash();
        _collider.isTrigger = false; // Re-enable collider
        OnStabEnd.Invoke();
    }
    private void EndOfStabAnimation() {
        isStabbing = false;
        swordMovement.OnEndAnimation.RemoveListener(EndOfStabAnimation);
        OnStabEnd.Invoke();
    }
}
