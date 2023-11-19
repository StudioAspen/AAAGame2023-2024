using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardStab : MonoBehaviour {

    [Header("References")]
    [SerializeField] SwordMovement swordMovement;

    [Header("Variables")]
    [SerializeField] float downwardStabAcceleration;
    [SerializeField] float downwardStabMaxSpeed;
    [SerializeField] float pressDownTime;
    [SerializeField] float bloodGain;

    //variables for downward stab
    float stabButtonTimer = 0.0f;
    bool canDownwardStab = true;
    bool isStabing = false;

    // Components
    Rigidbody rb;
    GroundCheck groundCheck;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        groundCheck = GetComponent<GroundCheck>();

        swordMovement.OnContact.AddListener(DownwardStabContact);
    }

    private void FixedUpdate() {
        if(isStabing) {
            DownwardStabMovementUpdate();
        }
    }

    private void Update() {

        //if grounded can perform downward stab
        canDownwardStab = !groundCheck.CheckOnGround();
    }
    public void TryDownwardStabUpdate() {
        if (canDownwardStab && !isStabing) {
            stabButtonTimer += Time.deltaTime;

            // Starting downward stab as long as the duration
            if (stabButtonTimer >= pressDownTime) {
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
        Vector3 addedVelocity = Vector3.down * downwardStabAcceleration;
        Vector3 maxVelocity = Vector3.down * downwardStabMaxSpeed;
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
