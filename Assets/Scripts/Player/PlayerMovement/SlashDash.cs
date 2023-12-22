using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SlashDash : MonoBehaviour {
    [Header("Events")]
    public UnityEvent OnEndSlashDash = new UnityEvent();

    [Header("References")]
    [SerializeField] DashCollider dashCollider;
    [SerializeField] SwordAnimation swordAnimation;
    [SerializeField] DashAction dashAction;

    // Components
    DashMovement dashMovement;
    Slash slash;

    //Variables
    bool isSlashDashing = false;

    private void Start() {
        dashMovement = GetComponent<DashMovement>();
        slash = GetComponent<Slash>();

        dashCollider.OnContact.AddListener(SlashDashContact);
        dashAction.OnDashEnd.AddListener(EndSlashDash);
    }

    public void StartSlashDash(Vector3 direction) {
        if (!isSlashDashing && dashMovement.CanDash()) {
            isSlashDashing = true;

            swordAnimation.StartSlashDashAnimation();
            dashMovement.PlayerInputDash(direction);
        }
    }
    private void EndSlashDash() {
        if(isSlashDashing) {
            swordAnimation.EndAnimation();
            isSlashDashing = false;
            OnEndSlashDash.Invoke();
        }
    }
    private void SlashDashContact(GameObject other) {
        if (isSlashDashing) {
            if(slash.SlashContactEffect(other)) {
                isSlashDashing = false;
            }
        }
    }
}
