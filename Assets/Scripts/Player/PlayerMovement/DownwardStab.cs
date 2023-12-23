using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardStab : MonoBehaviour {

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

    [Header("References")]
    [SerializeField] SwordAnimation swordAnimation;
    [SerializeField] PlayerPositionCheck playerPositionCheck;
    [SerializeField] MovementModification movementModification;
    [SerializeField] PlayerMovementStateManager playerMovementStateManager;

    // Components
    Rigidbody rb;
    Stab stab;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        stab = GetComponent<Stab>();

        swordAnimation.OnContact.AddListener(DownwardStabContact);
    }
    private void FixedUpdate() {
        if(playerMovementStateManager.currentState == PlayerMovementState.DOWNWARD_STAB) {
            DownwardStabMovementUpdate();
        }
    }
    private void Update() {

        //if grounded can perform downward stab
        canDownwardStab = !playerPositionCheck.CheckOnGround();
    }
    public void DownwardStabUpdate() {
        if (canDownwardStab && playerMovementStateManager.currentState == PlayerMovementState.IDLE) {
            stabButtonTimer += Time.deltaTime;
            Debug.Log(Time.deltaTime);

            // Starting downward stab as long as the duration
            if (stabButtonTimer >= pressDownTime) {
                stab.InterruptStab();
                playerMovementStateManager.ChangeState(PlayerMovementState.DOWNWARD_STAB);
                swordAnimation.StartDownwardStabAnimation();
            }
        }
    }

    public void ReleaseDownwardStab() {
        stabButtonTimer = 0;

        if (playerMovementStateManager.currentState == PlayerMovementState.DOWNWARD_STAB) {
            playerMovementStateManager.ChangeState(PlayerMovementState.IDLE);
            swordAnimation.EndAnimation();
        }
    }

    private void DownwardStabContact(GameObject other) {
        if(playerMovementStateManager.currentState == PlayerMovementState.DOWNWARD_STAB) {
            if(other.TryGetComponent(out DownwardStabEffect effect)) {
                effect.TriggerEffect();
                GetComponent<BloodThirst>().GainBlood(bloodGain, true);
            }
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
}
