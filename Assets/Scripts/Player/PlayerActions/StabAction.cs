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

        // Initalizing stab contact
        stabContact.ActivateContactEvent(swordMovement.OnContact, bloodGainAmount);
        
        // Demon sword variables
        swordMovement.OnEndAction.AddListener(EndOfStabAnimation);
        swordMovement.AttackPosition(attackDuration);
    }


    // This occurs at the end of the stab animation
    private void EndOfStabAnimation() {
        swordMovement.OnEndAction.RemoveListener(EndOfStabAnimation);
        EndAction();
    }

    // Checking if you can perform stab
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
