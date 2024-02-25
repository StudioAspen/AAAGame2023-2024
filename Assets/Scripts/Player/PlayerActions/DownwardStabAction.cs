using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardStabAction : MonoBehaviour {

    [Header("References")]
    [SerializeField] SwordMovement swordMovement;

    [Header("Movement")]
    [SerializeField] float downwardStabAcceleration;
    [SerializeField] float downwardStabMaxSpeed;
    [Header("Boosted Movement")]
    [SerializeField] float boostedDownwardStabAcceleration;
    [SerializeField] float boostedDownwardStabMaxSpeed;

    [Header("Other Variables")]
    [SerializeField] float pressDownTime;
    [SerializeField] float bloodGain;

    //variables for downward stab
    float stabButtonTimer = 0.0f;
    bool canDownwardStab = true;
    bool isStabing = false;

    // Components
    Rigidbody rb;
    PlayerPositionCheck playerPositionCheck;
    Stab stab;
    MovementModification movementModification;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        playerPositionCheck = GetComponent<PlayerPositionCheck>();
        stab = GetComponent<Stab>();
        movementModification = GetComponent<MovementModification>();

        swordMovement.OnContact.AddListener(DownwardStabContact);
    }

    private void FixedUpdate() {
        if(isStabing) {
            DownwardStabMovementUpdate();
        }
    }

    private void Update() {

        //if grounded can perform downward stab
        canDownwardStab = !playerPositionCheck.CheckOnGround();
    }
    public void TryDownwardStabUpdate() {
        if (canDownwardStab && !isStabing) {
            stabButtonTimer += Time.deltaTime;

            // Starting downward stab as long as the duration
            if (stabButtonTimer >= pressDownTime) {
                stab.InterruptStab();
                isStabing = true;
                swordMovement.DownwardAttackPosition();
            }
        }
    }
    public void ReleaseDownwardStab() {
        stabButtonTimer = 0;

        if (isStabing) {
            isStabing = false;
            swordMovement.EndAttackPosition();
        }
    }
    private void DownwardStabMovementUpdate() {
        Vector3 addedVelocity = Vector3.down * Mathf.Lerp(downwardStabAcceleration, boostedDownwardStabMaxSpeed, movementModification.boostForAll);
        Vector3 maxVelocity = Vector3.down * Mathf.Lerp(downwardStabMaxSpeed, boostedDownwardStabMaxSpeed, movementModification.boostForAll);
        Vector3 currentVerticalVelocity = new Vector3(0, rb.velocity.y, 0);

        // Applying vertical movement if the speed is higher than the max velocity
        float alignment = Vector3.Dot(currentVerticalVelocity / maxVelocity.magnitude, maxVelocity / maxVelocity.magnitude);
        if (alignment < 1) {
            rb.AddForce(addedVelocity, ForceMode.VelocityChange);
        }
    }

    private void DownwardStabContact(Collider other) {
        if(isStabing) {
            if(other.TryGetComponent<DownwardStabEffect>(out DownwardStabEffect effect)) {
                effect.TriggerEffect();
                GetComponent<BloodThirst>().GainBlood(bloodGain, true);
            }
        }
    }
}
