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
    [SerializeField] float terrainCheckScale;
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
    public RaycastHit CheckColldingWithTerrain(Vector3 direction) {
        // Debug.DrawLine(transform.position, transform.position + direction.normalized * (Mathf.Abs(collider.bounds.min.z - transform.position.z) + terrainCheckOffset)); // Ground Check
        // return Physics.Raycast(transform.position, direction.normalized, Mathf.Abs(playerCollider.bounds.min.z - transform.position.z) + terrainCheckOffset, ground);
        gizmoDir = direction;
        //bool check = Physics.BoxCast(transform.position, playerCollider.bounds.extents * terrainCheckScale, direction.normalized, out RaycastHit hitInfo, Quaternion.identity, terrainCheckOffset, ground);
        bool test = Physics.Raycast(new Ray(transform.position, direction), out RaycastHit hitInfo, terrainCheckOffset, ground);

        //Debug.Log(check);
        return hitInfo;
    }
    public bool CheckOnGround() {
        Vector3 extents = playerCollider.bounds.extents * groundCheckScale;
        //Debug.Log(playerCollider.bounds.extents * groundCheckScale);
        //extents.y = 1f;
        return Physics.BoxCast(transform.position, extents, Vector3.down, out RaycastHit hit, Quaternion.identity, playerCollider.bounds.extents.y + groundCheckOffset, ground);
        //return Physics.Raycast(transform.position, Vector3.down, Mathf.Abs(playerCollider.bounds.min.y - transform.position.y) + groundCheckOffset, ground);
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + (gizmoDir*terrainCheckOffset), (playerCollider.bounds.size * terrainCheckScale));
    }
}
