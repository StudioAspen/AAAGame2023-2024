using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGizmoDrawer : MonoBehaviour
{
    public float maxDistance = 500f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
