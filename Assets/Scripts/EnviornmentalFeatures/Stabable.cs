using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Stabable : MonoBehaviour
{

    public Vector3 dashDir; //direction of the dash
    public float dashLength; //how far the player is launched

    private void OnDrawGizmos()
    {
        //makes it so the gizmos transform with the local transform of the object
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;

        //direction is the direction and distance the player will dash through
        Vector3 direction = dashDir.normalized * dashLength;
        //ray starts behind transform.position so you can see it kinda go through the object
        Gizmos.DrawLine(-dashDir.normalized, direction);
        //handleLength determines how long the arrow handles are
        float handleLength = .5f;
        //got this math off the internet so i dont understand it too well but hey it works! x_x
        Vector3 rightHandle = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 200, 0) * new Vector3(0, 0, 1);
        Vector3 leftHandle = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 160, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(Vector3.zero + direction, rightHandle * handleLength);
        Gizmos.DrawRay(Vector3.zero + direction, leftHandle * handleLength);

    }
}
