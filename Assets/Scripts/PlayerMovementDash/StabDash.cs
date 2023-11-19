using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] DashCollider dashCollider;

    [Header("Events")]
    public UnityEvent OnEndStabDash = new UnityEvent();

    // Components
    DashMovement dashMovement;
    Stab stab;

    //Variables
    bool isDashing = false;

    private void Start() {
        dashMovement = GetComponent<DashMovement>();
        stab = GetComponent<Stab>();

        dashCollider.OnContact.AddListener(StabDashContact);
    }

    public void TryStartStabDash(Vector3 direction) {
        if(!isDashing) {
            isDashing = true;

            dashMovement.OnDashEnd.AddListener(EndStabDash);
            dashMovement.TryPlayerInputDash(direction);
        }
    }
    private void EndStabDash() {
        dashMovement.OnDashEnd.RemoveListener(EndStabDash);
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
