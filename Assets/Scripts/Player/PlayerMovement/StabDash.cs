using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SwordAnimation swordAnimation;
    [SerializeField] DashCollider dashCollider;

    [Header("Events")]
    public UnityEvent OnEndStabDash = new UnityEvent();

    // Components
    DashMovement dashMovement;
    Stab stab;

    //Variables
    bool isDashing = false;

    private void Start() {
        // Getting components
        dashMovement = GetComponent<DashMovement>();
        stab = GetComponent<Stab>();

        dashCollider.OnContact.AddListener(StabDashContact);
    }

    public void TryStartStabDash(Vector3 direction) {
        if(!isDashing) {
            isDashing = true;

            swordAnimation.StartStabDashAnimation();
            dashMovement.OnDashEnd.AddListener(EndStabDash);
            dashMovement.PlayerInputDash(direction);
        }
    }
    private void EndStabDash() {
        dashMovement.OnDashEnd.RemoveListener(EndStabDash);
        swordAnimation.EndAnimation();
        isDashing = false;
        OnEndStabDash.Invoke();
    }
    private void StabDashContact(GameObject other) {
        if(isDashing) {
            if (other.TryGetComponent(out StabableTerrain stabable)) {
                isDashing = false;
                stab.DashThrough(stabable);
            }
        }
    }
}
