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
    [SerializeField] float terrainCheckScaleXZ;
    [SerializeField] float terrainCheckScaleY;
    [SerializeField] float terrainCastScaleZ;
    [Range(0f, 1f)]
    [SerializeField] float terrainCastScaleXY;
    [Range(0f, 1f)]
    [SerializeField] float groundCheckScaleXZ;
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
    public bool TryGetClosestColliders(Vector3 direction, out Collider[] colliders) {
        // Scaling Y extents seperatly
        Vector3 extents = new Vector3(playerCollider.bounds.extents.x*terrainCheckScaleXZ, 
            playerCollider.bounds.extents.y*terrainCheckScaleY, 
            playerCollider.bounds.extents.z*terrainCheckScaleXZ);

        // Checking colliders
        if(direction.magnitude <= 0) {
            direction = transform.forward;
        }
        Collider[] foundColliders = Physics.OverlapBox(transform.position, extents, Quaternion.LookRotation(direction.normalized), ground);
        colliders = foundColliders;
        if(foundColliders.Length > 0) {
            return true;
        }
        else {
            return false;
        }
    }

    // Checking collision to calculate force perpendicular to the surface of collider (for sticky wall bug)
    // THIS WILL NO LONGER BE USED THERE IS AN EASIER IMPLEMENTATION
    public Vector3 CorrectVelocityCollision(Vector3 addedVelocity) {
        
        if (TryGetClosestColliders(addedVelocity, out Collider[] colliders)) {
            // Calculate velocity based on colliders
            foreach (Collider collider in colliders) {
                Vector3 normal = transform.position-collider.ClosestPoint(transform.position);

                // Calculating velocity
                if (Vector3.Dot(normal.normalized, addedVelocity.normalized) < 0) { // Checking if the added velocity is going into the surface
                                                                                    // Finding perpendicular vector to the surface normal
                    Vector3 rotationVector = Vector3.Cross(normal.normalized, addedVelocity.normalized);
                    Vector3 perpendicularToNormal = MyMath.RotateAboutAxis(normal, rotationVector, 90f);

                    Debug.DrawLine(transform.position, transform.position + normal, Color.blue);
                    Debug.DrawLine(transform.position, transform.position + perpendicularToNormal, Color.green);

                    float perpendicularProjection = Vector3.Dot(perpendicularToNormal.normalized, addedVelocity);
                    //float perpendicularProjection = addedVelocity.magnitude;
                    addedVelocity = perpendicularProjection * perpendicularToNormal.normalized;
                }
            }
        }

        return addedVelocity;
    }
    public bool CheckOnGround() {
        Vector3 extents = new Vector3(playerCollider.bounds.extents.x * groundCheckScaleXZ,
            playerCollider.bounds.extents.y,
            playerCollider.bounds.extents.z * groundCheckScaleXZ);
        return Physics.OverlapBox(transform.position + (Vector3.down * groundCheckOffset), extents, Quaternion.LookRotation(transform.forward) , ground).Length > 0;
    }
    private void OnDrawGizmos() {
        //Gizmos.DrawWireCube(transform.position + (gizmoDir*terrainCheckOffset), (playerCollider.bounds.size * terrainCheckScale));
    }
}
