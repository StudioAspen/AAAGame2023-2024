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
    // Returns the rotated angle based on inputed angle
    public static Vector2 RotateAngle(Vector2 v, float angle) {
        float rad = angle * Mathf.Deg2Rad;
        Vector2 v1 = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        Vector2 v2 = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));
        return v.x*v1 + v.y*v2;
    }

    // Returns 1 or -1 based on side of source vector (if they are perpendicular default is 1)
    public static int OnSide(Vector2 source, Vector2 check) {
        int output;

        source = RotateAngle(source, 90);
        output = (int) Mathf.Sign(Vector2.Dot(source, check));

        if(output == 0) {
            return 1;
        }
        
        return output;
    }
}
