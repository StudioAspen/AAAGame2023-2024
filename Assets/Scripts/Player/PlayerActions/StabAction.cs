using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class StabAction : PlayerAction {
    [Header("References")]
    [SerializeField] SwordMovement swordMovement;

    [Header("Other Variables")]
    [SerializeField] float dashSpeed;
    [SerializeField] float boostedDashSpeed;
    [SerializeField] float bloodGainAmount;
    [SerializeField] float attackDuration;

    // Movement Compoenents
    DashAction dashAction;
    DashThroughAction dashThroughAction;
    BasicMovementAction movementAction;
    MovementModification movementModification;

    // Variables
    bool isStabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        dashAction = GetComponent<DashAction>();
        dashThroughAction = GetComponent<DashThroughAction>();
        movementAction = GetComponent<BasicMovementAction>();
        movementModification = GetComponent<MovementModification>();

        dashThroughAction.OnEndAction.AddListener(EndOfDash);

        // Setting up Demon Sword
        swordMovement.OnContact.AddListener(StabContact);
    }


    public void StabInput()
    {
        //Animation Stuff (to be implemented later)
        isStabbing = true;

        // Demon sword variables
        swordMovement.OnEndAction.AddListener(EndOfStabAnimation);
        swordMovement.AttackPosition(attackDuration);
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
                EndAction();
                GetComponent<PlayerActionManager>().ChangeAction(dashAction);
                dashThroughAction.DashThrough(enviornment);
            }
        }
    }

    private void EndOfDash() {
        movementAction.ResetJump();
        dashAction.ResetDash();
    }
    
    private void EndOfStabAnimation() {
        swordMovement.OnEndAction.RemoveListener(EndOfStabAnimation);
        EndAction();
    }

    public bool CanPerformStab() {
        return !isStabbing;
    }

    public override void EndAction() {
        isStabbing = false;
        swordMovement.EndAttackPosition();
        OnEndAction.Invoke();
    }
}
