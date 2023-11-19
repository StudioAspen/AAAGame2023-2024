using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject swordObject;
    [SerializeField] DashCollider dashCollider;

    [Header("Events")]
    public UnityEvent OnEndStabDash = new UnityEvent();

    // Components
    DashMovement dashMovement;
    DemonSword sword;
    Stab stab;

    //Variables
    bool isDashing = false;

    private void Start() {
        // Getting components
        dashMovement = GetComponent<DashMovement>();
        stab = GetComponent<Stab>();
        sword = swordObject.GetComponent<DemonSword>();

        dashCollider.OnContact.AddListener(StabDashContact);
    }

    public void TryStartStabDash(Vector3 direction) {
        if(!isDashing) {
            Debug.Log("starting stabdash");
            isDashing = true;

            sword.DashAttackPosition();
            dashMovement.OnDashEnd.AddListener(EndStabDash);
            dashMovement.TryPlayerInputDash(direction);
        }
    }
    private void EndStabDash() {
        dashMovement.OnDashEnd.RemoveListener(EndStabDash);
        sword.EndAttackPosition();
        isDashing = false;
        OnEndStabDash.Invoke();
    }
    private void StabDashContact(Collider other) {
        if(isDashing) {
            if (other.TryGetComponent(out Stabable stabable)) {
                isDashing = false;
                stab.DashThrough(stabable);
            }
        }
    }
}
