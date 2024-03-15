using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycast : MonoBehaviour {
    Collider collide;
    [SerializeField]Transform endPoint;
    [SerializeField] float maxDis;
    [SerializeField] LayerMask ground;
    private void Start() {
        collide = GetComponent<Collider>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = endPoint.position - transform.position;
        bool test = Physics.BoxCast(transform.position, collide.bounds.extents, direction, out RaycastHit hitInfo, Quaternion.identity, maxDis, ground);
        //bool test = Physics.Raycast(new Ray(transform.position, direction), out RaycastHit hitInfo, maxDis, ground);
        if (hitInfo.point != Vector3.zero) {
            Debug.DrawLine(transform.position, hitInfo.point, Color.blue, 1f);
        }
        Debug.Log(test);
    }
}
