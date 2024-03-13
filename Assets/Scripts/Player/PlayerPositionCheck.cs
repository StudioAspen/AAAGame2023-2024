using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionCheck : MonoBehaviour {
    // References
    //Rigidbody rb;

    // Components
    [SerializeField] Collider playerCollider;

    //Dedicated ground check
    [Header("Checks")]
    [SerializeField] LayerMask ground;
    [SerializeField] float groundCheckOffset;
    [SerializeField] float terrainCheckOffset;
    [SerializeField] float terrainCheckScaleXZ;
    [SerializeField] float terrainCheckScaleY;
    [Range(0f, 1f)]
    [SerializeField] float groundCheckScale;
    public bool grounded;

    Vector3 gizmoDir;
    private void Start() {
        //rb = playerCollider.GetComponent<Rigidbody>();
    }
    private void Update() {
        grounded = CheckOnGround();
    }
    public void WallStickCheck(Collision collision, Rigidbody rb) {
        ContactPoint[] points = new ContactPoint[collision.contactCount];

        Vector3 average = Vector3.zero;
        foreach (ContactPoint point in collision.contacts) {
            average += point.normal;
        }
        average = average / points.Length;

        Vector3 rotatedAngle = MyMath.RotateXZAngle(average, 90f);
        float alignment = Vector3.Dot(rotatedAngle, rb.velocity);
        
    }

    // Checks whats colliding with and returning the closest collider
    public bool TryGetNormalOfClosestHoriontalCollider(Vector3 direction, out Vector3 normal) {
        // Scaling Y extents seperatly
        Vector3 extents = new Vector3(playerCollider.bounds.extents.x*terrainCheckScaleXZ, 
            playerCollider.bounds.extents.y*terrainCheckScaleY, 
            playerCollider.bounds.extents.z*terrainCheckScaleXZ);

        // Checking colliders
        if(direction.magnitude <= 0) {
            direction = transform.forward;
        }
        Collider[] colliders = Physics.OverlapBox(transform.position, extents, Quaternion.LookRotation(direction.normalized), ground);
        
        if(colliders.Length == 0) {
            normal = Vector3.zero;
            return false;
        }

        // Finding clostest collider
        Collider closestCollider = null;
        float closestDist = float.MaxValue;
        foreach(Collider collider in colliders) {
            float valCheck = (collider.ClosestPoint(transform.position) - transform.position).magnitude;
            if (valCheck < closestDist) {
                closestCollider = collider; 
                closestDist = valCheck; 
            }
        }

        // Getting normal from
        Ray rayToCollider = new Ray(transform.position, closestCollider.ClosestPoint(transform.position)-transform.position);
        closestCollider.Raycast(rayToCollider, out RaycastHit hit, extents.magnitude);

        
        // output
        normal = hit.normal;
        return true;
    }
    public bool CheckOnGround() {
        return Physics.OverlapBox(transform.position + (Vector3.down * groundCheckOffset), playerCollider.bounds.extents * groundCheckScale, Quaternion.identity, ground).Length > 0;
    }
    private void OnDrawGizmos() {
        //Gizmos.DrawWireCube(transform.position + (gizmoDir*terrainCheckOffset), (playerCollider.bounds.size * terrainCheckScale));
    }
}
