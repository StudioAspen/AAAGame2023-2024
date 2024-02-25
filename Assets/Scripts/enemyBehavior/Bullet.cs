using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 moveForce;
    public Rigidbody rigidBody;
    public float damage;

    private void FixedUpdate()
    {
        rigidBody.AddForce(moveForce * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<BloodThirst>().LoseBlood(damage);
    }
}