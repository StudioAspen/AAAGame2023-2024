using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionCheck : MonoBehaviour {
    // Components
    Collider playerCollider;

    //Dedicated ground check
    [Header("Checks")]
    [SerializeField] LayerMask ground;
    [SerializeField] float groundCheckOffset;
    [SerializeField] float terrainCheckOffset;
    private void Start() {
        playerCollider = transform.parent.GetComponent<Collider>();
    }
    public bool CheckColldingWithTerrain(Vector3 direction) {
        //Debug.DrawLine(transform.position, transform.position + direction.normalized * (Mathf.Abs(collider.bounds.min.z - transform.position.z) + terrainCheckOffset)); // Ground Check
        return Physics.Raycast(transform.position, direction.normalized, Mathf.Abs(playerCollider.bounds.min.z - transform.position.z) + terrainCheckOffset, ground);
    }
    public bool CheckOnGround() {
        return Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(playerCollider.bounds.min.y - transform.position.y) + groundCheckOffset, ground); ;
    }
}
