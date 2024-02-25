using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class StabAction : PlayerAction {
    [Header("References")]
    [SerializeField] SwordMovement swordMovement;

    [Header("Other Variables")]
    [SerializeField] float bloodGainAmount;
    [SerializeField] float attackDuration;

    // Movement Compoenents
    StabContact stabContact;

    // Variables
    bool isStabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        // Getting components
        stabContact = GetComponentInChildren<StabContact>();
    }


    public void StabInput()
    {
        //Animation Stuff (to be implemented later)
        isStabbing = true;

        // Demon sword variables
        stabContact.ActivateContactEvent(swordMovement.OnContact, EndAction, bloodGainAmount);
        swordMovement.OnEndAction.AddListener(EndOfStabAnimation);
        swordMovement.AttackPosition(attackDuration);
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
        if (swordMovement.isAttacking) {
            swordMovement.EndAttackPosition();
        }
        stabContact.EndContactEvent();
        OnEndAction.Invoke();
    }
}
