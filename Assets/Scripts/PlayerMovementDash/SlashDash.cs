using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class SlashDash : MonoBehaviour {
    [Header("References")]
    [SerializeField] DashCollider dashCollider;
    [SerializeField] GameObject swordObject;

    [Header("Events")]
    public UnityEvent OnEndSlashDash = new UnityEvent();

    // Components
    DashMovement dashMovement;
    Slash slash;
    DemonSword sword;

    //Variables
    bool isDashing = false;

    private void Start() {
        dashMovement = GetComponent<DashMovement>();
        slash = GetComponent<Slash>();
        sword = swordObject.GetComponent<DemonSword>();

        dashCollider.OnContact.AddListener(SlashDashContact);
    }

    public void TryStartSlashDash(Vector3 direction) {
        if (!isDashing) {
            isDashing = true;

            sword.DashAttackPosition();
            dashMovement.OnDashEnd.AddListener(EndSlashDash);
            dashMovement.TryPlayerInputDash(direction);
        }
    }
    private void EndSlashDash() {
        dashMovement.OnDashEnd.RemoveListener(EndSlashDash);
        sword.EndAttackPosition();
        isDashing = false;
        OnEndSlashDash.Invoke();
    }
    private void SlashDashContact(Collider other) {
        if (isDashing) {
            if (other.TryGetComponent(out Slashable slashable) &&
                other.TryGetComponent(out PathCreator pc)) {
                isDashing = false;
                slash.StartSlide(slashable, pc, other);
            }
        }
    }
}
