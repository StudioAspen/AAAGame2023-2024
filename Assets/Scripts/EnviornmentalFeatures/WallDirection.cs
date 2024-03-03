using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDirection : MonoBehaviour
{
    public Vector3 GetForwardVector()
    {
        return transform.forward;
    }
}
