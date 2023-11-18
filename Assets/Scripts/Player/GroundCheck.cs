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
    bool onGround = false;

    private void Start() {
        collider = GetComponent<Collider>();
    }
    private void Update() {
        onGround = Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(collider.bounds.min.y - transform.position.y) + groundCheckOffset, ground);
    }
    public bool CheckOnGround() {
        return onGround;
    }
}
