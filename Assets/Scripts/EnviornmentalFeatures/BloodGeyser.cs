using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodGeyser : MonoBehaviour
{
    float acceleration;
    float maxSpeed;
    public void SetStats(float setAcceleration, float setMaxSpeed) {
        acceleration = setAcceleration;
        maxSpeed = setMaxSpeed;
    }

    private void OnTriggerStay(Collider other) {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            if (rb.velocity.y < maxSpeed) {
                rb.AddForce(Vector3.up * acceleration, ForceMode.VelocityChange);
            }
        }
    }
}
