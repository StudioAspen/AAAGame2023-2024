using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SlashDash : MonoBehaviour {
    [Header("References")]
    [SerializeField] DashCollider dashCollider;
    [SerializeField] SwordAnimation swordAnimation;

    [Header("Events")]
    public UnityEvent OnEndSlashDash = new UnityEvent();

    // Components
    DashMovement dashMovement;
    Slash slash;

    //Variables
    bool isDashing = false;

    private void Start() {
        dashMovement = GetComponent<DashMovement>();
        slash = GetComponent<Slash>();

        dashCollider.OnContact.AddListener(SlashDashContact);
    }

    public void TryStartSlashDash(Vector3 direction) {
        if (!isDashing) {
            isDashing = true;

            swordAnimation.StartSlashDashAnimation();
            dashMovement.OnDashEnd.AddListener(EndSlashDash);
            dashMovement.PlayerInputDash(direction);
        }
    }
    private void EndSlashDash() {
        dashMovement.OnDashEnd.RemoveListener(EndSlashDash);
        swordAnimation.EndAnimation();
        isDashing = false;
        OnEndSlashDash.Invoke();
    }
    private void SlashDashContact(GameObject other) {
        if (isDashing) {
            if (other.TryGetComponent(out SlashableTerrain slashable) &&
                other.TryGetComponent(out PathCreator pc)) {
                isDashing = false;
                slash.StartSlide(pc, other);
            }
        }
    }
}
