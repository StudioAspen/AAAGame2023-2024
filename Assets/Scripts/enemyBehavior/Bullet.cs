using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 moveForce;
    public Rigidbody rigidBody;

    private void FixedUpdate()
    {
        rigidBody.AddForce(moveForce * Time.deltaTime);
    }
}
