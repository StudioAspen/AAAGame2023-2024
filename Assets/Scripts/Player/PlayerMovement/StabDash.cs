using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabDash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SwordAnimation swordAnimation;
    [SerializeField] DashCollider dashCollider;
    [SerializeField] DashAction dashAction;

    [Header("Events")]
    public UnityEvent OnEndStabDash = new UnityEvent();

    // Components
    DashMovement dashMovement;
    Stab stab;

    //Variables
    bool isStabDashing = false;

    private void Start() {
        // Getting components
        dashMovement = GetComponent<DashMovement>();
        stab = GetComponent<Stab>();

        dashCollider.OnContact.AddListener(StabDashContact);
        dashAction.OnDashEnd.AddListener(EndStabDash);
    }

    public void StartStabDash(Vector3 direction) {
        if(!isStabDashing && dashMovement.CanDash()) {
            isStabDashing = true;

            swordAnimation.StartStabDashAnimation();

            dashMovement.PlayerInputDash(direction);
        }
    }

    private void EndStabDash() {
        if (isStabDashing) {
            swordAnimation.EndAnimation();
            isStabDashing = false;
            OnEndStabDash.Invoke();
        }
    }

    private void StabDashContact(GameObject other) {
        if(isStabDashing) {
            if(stab.StabContactEffect(other)) {
                isStabDashing = false;
                dashAction.InterruptDash();
            }
        }
    }
}
