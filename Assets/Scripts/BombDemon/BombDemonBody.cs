using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDemonBody : MonoBehaviour
{
    public BombDemon bombDemon;
    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void KinematicTurn(bool active)
    {
        rigidbody.isKinematic = active;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            bombDemon.CollisionDetect(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            bombDemon.CollisionDetect(false);
        }
    }
  


}
