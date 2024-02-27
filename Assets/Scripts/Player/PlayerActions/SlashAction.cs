using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;
using UnityEngine.UIElements;

public class SlashAction : PlayerAction
{
    [Header("References")]
    [SerializeField] SwordMovement swordMovement;

    [Header("Other Variables")]
    public float bloodGained;
    public float attackDuration;

    // Components
    private SlashContact slashContact;

    private bool isSlashing = false;

    private void Start()
    {
        // Getting Components
        slashContact = GetComponentInChildren<SlashContact>();
    }

    public void SlashInput() {
        //Animation Stuff (to be implemented later)
        isSlashing = true;

        // Demon sword variables
        slashContact.ActivateContactEvent(swordMovement.OnContact, EndAction, bloodGained);
        swordMovement.OnEndAction.AddListener(EndOfSlashAnimation);
        swordMovement.SlashPosition(attackDuration);
    }

    private void EndOfSlashAnimation() {
        swordMovement.OnEndAction.RemoveListener(EndOfSlashAnimation);
        EndAction();
    }
    public bool CanPerformSlash() {
        return !isSlashing;
    }
    public override void EndAction() {
        isSlashing = false;
        if(swordMovement.isAttacking) {
            swordMovement.EndAttackPosition();
        }
        slashContact.EndContactEvent();
        OnEndAction.Invoke();
    }
}
