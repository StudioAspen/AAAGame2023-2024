using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMath : MonoBehaviour
{
    // Finds the angle () between two vectors 
    public static float AngleBetweenVectors(Vector3 v1, Vector3 v2) {
        float top = Vector3.Dot(v1, v2);
        float bottom = v1.magnitude * v2.magnitude;

        return Mathf.Acos(top/bottom);
    }
}
