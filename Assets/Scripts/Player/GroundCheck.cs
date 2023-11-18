using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    //References
    Collider collider;
    
    //Dedicated ground check
    [Header("Ground Check")]
    public LayerMask ground;
    public float groundCheckOffset;

    private void Start() {
        collider = GetComponent<Collider>();
    }
    public bool CheckOnGround() {
        return Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(collider.bounds.min.y - transform.position.y) + groundCheckOffset, ground);
    }
}
