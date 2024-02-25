using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDashAction : PlayerAction
{
    [Header("References")]
    [SerializeField] SwordMovement swordMovement;
    [SerializeField] DashCollider dashCollider;

    [Header("Events")]
    public UnityEvent OnEndStabDash = new UnityEvent();

    // Components
    DashMovement dashMovement;

    //Variables
    bool isDashing = false;

    private void Start() {
        // Getting components
        dashMovement = GetComponent<DashMovement>();

        dashCollider.OnContact.AddListener(StabDashContact);
    }

    public void StabDashInput(Vector3 direction) {
        if(!isDashing) {
            isDashing = true;

            swordMovement.DashAttackPosition();
            dashMovement.OnDashEnd.AddListener(EndStabDash);
            //dashMovement.DashInput(direction);
        }
    }
    private void EndStabDash() {
        dashMovement.OnDashEnd.RemoveListener(EndStabDash);
        swordMovement.EndAttackPosition();
        isDashing = false;
        OnEndStabDash.Invoke();
    }
    private void StabDashContact(Collider other) {
        if(isDashing) {
            if (other.TryGetComponent(out Stabable stabable)) {
                stabable.TriggerEffect();
            }
            if(other.TryGetComponent(out StabableEnviornment enviornment)) {
                isDashing = false;
                //stab.DashThrough(enviornment);
            }
        }
    }

    public override void EndAction() {
        OnEndAction.Invoke();
    }
}
