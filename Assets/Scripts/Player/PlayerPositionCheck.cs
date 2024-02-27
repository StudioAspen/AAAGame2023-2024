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

    Vector3 directionTest;
    public bool CheckColldingWithTerrain(Vector3 direction) {
        // Debug.DrawLine(transform.position, transform.position + direction.normalized * (Mathf.Abs(collider.bounds.min.z - transform.position.z) + terrainCheckOffset)); // Ground Check
        // return Physics.Raycast(transform.position, direction.normalized, Mathf.Abs(playerCollider.bounds.min.z - transform.position.z) + terrainCheckOffset, ground);
        directionTest = direction;
        Vector3 terrainCheckHalfSize = (playerCollider.bounds.size * terrainCheckScale)/ 2;
        return Physics.BoxCast(transform.position, terrainCheckHalfSize, direction, Quaternion.identity, terrainCheckOffset, ground);
    }
    public bool CheckOnGround() {
        return Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(playerCollider.bounds.min.y - transform.position.y) + groundCheckOffset, ground); ;
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + (directionTest*terrainCheckOffset), playerCollider.bounds.size * terrainCheckScale);
    }
}
