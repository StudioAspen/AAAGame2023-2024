using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Stab : MonoBehaviour {
    [Header("Other Variables")]
    [SerializeField] float dashSpeed;
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float bloodGainAmount;
    [SerializeField] float attackDuration;

    [Header("Events")]
    public UnityEvent OnStabEnd = new UnityEvent();

    [Header("References")]
    [SerializeField] SwordAnimation swordAnimation;
    [SerializeField] DashAction dashAction;
    [SerializeField] MovementModification movementModification;
    [SerializeField] BloodThirst bloodThirst; 

    // Movement Components and References
    DashMovement dashMovement;
    PlayerMovement playerMovement;
    Slash slash;
    Collider _collider;
    Rigidbody rb;

    // Variables
    public bool isStabbing = false;

    // Start is called before the first frame update
    void Start() {
        // Getting components
        dashMovement = GetComponent<DashMovement>();
        playerMovement = GetComponent<PlayerMovement>();
        slash = GetComponent<Slash>();
        _collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        // Setting up Demon Sword
        swordAnimation.OnContact.AddListener(StabContact);
        swordAnimation.OnEndAnimation.AddListener(EndOfStabAnimation);
    }


    public void StartStab() {
        if (!isStabbing && !slash.isSlashing) {
            isStabbing = true;

            swordAnimation.StartStabAnimation();
        }
    }

    public void InterruptStab() {
        if(isStabbing) {
            swordAnimation.EndAnimation();
        }
    }

    public void StabContact(GameObject other)
    {
        if(isStabbing) {
            if(StabContactEffect(other)) {
                isStabbing = false;
            }
        }
    }

    public bool StabContactEffect(GameObject other) {
        bool found = false;
        if (other.TryGetComponent(out StabableTerrain stabableTerrain)) {
            // Setting up and starting dash
            DashThrough(stabableTerrain);
            found = true;
        }

        // Triggering generic effects
        if (other.TryGetComponent(out StabbedEffect stabbedEffect)) {
            stabbedEffect.TriggerEffect();
            found = true;
        }

        if(found) {
            bloodThirst.GainBlood(bloodGainAmount, true);
        }
        return found;
    }

    public void DashThrough(StabableTerrain stabableTerrain) {
        _collider.isTrigger = true; // Setting as trigger to prevent collisions

        // Dashing
        dashAction.OnDashEnd.AddListener(EndOfDash);
        float dashDuration = (stabableTerrain.dashLength / Mathf.Lerp(dashSpeed, boostedDashSpeed, movementModification.boostForAll));
        rb.position = stabableTerrain.dashStartTransform.position;
        dashAction.Dash(stabableTerrain.dashLength, dashDuration, stabableTerrain.dashDir);
    }
    private void EndOfDash() {
        dashAction.OnDashEnd.RemoveListener(EndOfDash);
        _collider.isTrigger = false; // Re-enable collider allowing collisions

        playerMovement.ResetJump();
        dashMovement.ResetDash();
        
        OnStabEnd.Invoke();
    }
    private void EndOfStabAnimation() {
        isStabbing = false;
        OnStabEnd.Invoke();
    }
}
