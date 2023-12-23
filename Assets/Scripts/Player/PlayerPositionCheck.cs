using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionCheck : MonoBehaviour {
    //Dedicated ground check
    [Header("Checks")]
    [SerializeField] LayerMask ground;
    [SerializeField] float groundCheckOffset;
    [SerializeField] float terrainCheckOffset;
    // Components
    [Header("References")]
    [SerializeField] Collider _collider;

    public bool CheckColldingWithTerrain(Vector3 direction) {
        //Debug.DrawLine(transform.position, transform.position + direction.normalized * (Mathf.Abs(collider.bounds.min.z - transform.position.z) + terrainCheckOffset)); // Ground Check
        return Physics.Raycast(transform.position, direction.normalized, Mathf.Abs(_collider.bounds.min.z - transform.position.z) + terrainCheckOffset, ground);
    }
    public bool CheckOnGround() {
        return Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(_collider.bounds.min.y - transform.position.y) + groundCheckOffset, ground);
    }
}
