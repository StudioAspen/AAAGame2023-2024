using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Stab : MonoBehaviour {
    [Header("References")]
    [SerializeField] SwordMovement swordMovement;

    [Header("Other Variables")]
    [SerializeField] float dashSpeed;
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float bloodGainAmount;
    [SerializeField] float attackDuration;

    [Header("Events")]
    public UnityEvent OnStabEnd = new UnityEvent();

    // Movement Compoenents
    DashAction dashAction;
    DashMovement dashMovement;
    PlayerMovement playerMovement;
    Collider collider;
    Rigidbody rb;
    MovementModification movementModification;
    PlayerStateManager stateManager;

    // Variables
    bool isStabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        dashAction = GetComponentInChildren<DashAction>();
        dashMovement = GetComponent<DashMovement>();
        playerMovement = GetComponent<PlayerMovement>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        movementModification = GetComponent<MovementModification>();
        stateManager = GetComponentInChildren<PlayerStateManager>();

        // Setting up Demon Sword
        swordMovement.OnContact.AddListener(StabContact);
    }


    public void StabInput()
    {
        if (!isStabbing) {
            //Animation Stuff (to be implemented later)
            isStabbing = true;

            // Demon sword variables
            swordMovement.OnEndAction.AddListener(EndOfStabAnimation);
            swordMovement.AttackPosition(attackDuration);
        }
    }

    public void InterruptStab()
    {
        isStabbing = false;
        swordMovement.EndAttackPosition();
    }

    public void StabContact(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Stabable stabable))
        {
            if(isStabbing)
            {
                stabable.TriggerEffect();
            }
        }
        if (other.gameObject.TryGetComponent(out StabableEnviornment enviornment)) {
            if (isStabbing) {
                // Setting proper listeners and variables
                isStabbing = false;
                swordMovement.OnEndAction.RemoveListener(EndOfStabAnimation);

                // Setting up and starting dash
                DashThrough(enviornment);
            }
        }
    }
    public void DashThrough(StabableEnviornment stabableEnviornment) {
        GetComponent<BloodThirst>().GainBlood(bloodGainAmount, true);
        dashAction.OnDashEnd.AddListener(EndOfDash);
        collider.isTrigger = true; // Setting as trigger to prevent collisions
        float dashDuration = (stabableEnviornment.dashLength / Mathf.Lerp(dashSpeed, boostedDashSpeed, movementModification.boostForAll));
        rb.position = stabableEnviornment.dashStartTransform.position;
        dashAction.Dash(stabableEnviornment.dashLength, dashDuration, stabableEnviornment.dashDir);
    }
    private void EndOfDash() {
        dashAction.OnDashEnd.RemoveListener(EndOfDash);
        playerMovement.ResetJump();
        dashMovement.ResetDash();
        collider.isTrigger = false; // Re-enable collider
        OnStabEnd.Invoke();
    }
    private void EndOfStabAnimation() {
        isStabbing = false;
        swordMovement.OnEndAction.RemoveListener(EndOfStabAnimation);
        OnStabEnd.Invoke();
    }
}
