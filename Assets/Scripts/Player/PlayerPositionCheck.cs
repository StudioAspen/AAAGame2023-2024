using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionCheck : MonoBehaviour {
    // Components
    Collider collider;
    
    //Dedicated ground check
    [Header("Checks")]
    [SerializeField] LayerMask ground;
    [SerializeField] float groundCheckOffset;
    [SerializeField] float terrainCheckOffset;
    bool onGround = false;

    private void Start() {
        collider = GetComponent<Collider>();
    }
    private void Update() {
        onGround = Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(collider.bounds.min.y - transform.position.y) + groundCheckOffset, ground);
    }
    public bool CheckColldingWithTerrain(Vector3 direction) {
        return Physics.Raycast(transform.position, direction.normalized, Mathf.Abs(collider.bounds.min.z - transform.position.z) + terrainCheckOffset, ground);
    }
    public bool CheckOnGround() {
        return onGround;
    }
}
