using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionCheck : MonoBehaviour {
    // Components
    [SerializeField] Collider playerCollider;

    //Dedicated ground check
    [Header("Checks")]
    [SerializeField] LayerMask ground;
    [SerializeField] float groundCheckOffset;
    [SerializeField] float terrainCheckOffset;
    [Range(0f,1f)]
    [SerializeField] float terrainCheckScale;
    [Range(0f, 1f)]
    [SerializeField] float groundCheckScale;
    public bool grounded;
    private void Update() {
        grounded = CheckOnGround();
    }
    public bool CheckColldingWithTerrain(Vector3 direction) {
        // Debug.DrawLine(transform.position, transform.position + direction.normalized * (Mathf.Abs(collider.bounds.min.z - transform.position.z) + terrainCheckOffset)); // Ground Check
        // return Physics.Raycast(transform.position, direction.normalized, Mathf.Abs(playerCollider.bounds.min.z - transform.position.z) + terrainCheckOffset, ground);
        Vector3 terrainCheckHalfSize = (playerCollider.bounds.size * terrainCheckScale)/ 2;
        return Physics.BoxCast(transform.position, terrainCheckHalfSize, direction, Quaternion.identity, terrainCheckOffset, ground);
    }
    public bool CheckOnGround() {
        return Physics.OverlapBox(transform.position+(Vector3.down*groundCheckOffset), playerCollider.bounds.extents*groundCheckScale, Quaternion.identity, ground).Length > 0;

        //return Physics.SphereCast(transform.position, playerCollider.bounds.extents.x*groundCheckScale, Vector3.down, out RaycastHit hit, playerCollider.bounds.extents.y + groundCheckOffset, ground);
        //return Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(playerCollider.bounds.min.y - transform.position.y) + groundCheckOffset, ground);
    }
}
