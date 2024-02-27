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
    public float timer = 0; // Used for slash dash

    private void Start()
    {
        // Getting Components
        slashContact = GetComponentInChildren<SlashContact>();
    }

    private void Update() {
        if(isSlashing) {
            timer += Time.deltaTime;
        }
    }

    public void SlashInput() {
        //Animation Stuff (to be implemented later)
        isSlashing = true;
        timer = 0;

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
