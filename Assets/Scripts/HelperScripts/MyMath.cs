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
    // Returns the rotated angle based on inputed angle
    public static Vector3 RotateXZAngle(Vector3 v, float angle) {
        Vector2 holder = RotateAngle(new Vector2(v.x, v.z), angle);


        return new Vector3(holder.x, v.y, holder.y);
    }

    // Rotates vector v about the axis by a certain amount of degrees
    public static Vector3 RotateAboutAxis(Vector3 v, Vector3 axis, float angle) {
        Vector3 v1 = GetComponentOf(v, axis);
        Vector3 v2 = v - v1;
        
        Vector3 v3 = Vector3.Cross(axis, v2);

        float x1 = Mathf.Cos(angle * Mathf.Deg2Rad) / v2.magnitude;
        float x2 = Mathf.Sin(angle * Mathf.Deg2Rad) / v3.magnitude;
        Vector3 v4 = v2.magnitude * (x1 * v2 + x2 * v3);

        return v4 + v1;
    }

    // Returns the a component along b
    public static Vector3 GetComponentOf(Vector3 a, Vector3 b) {
        return (Vector3.Dot(a, b) / Vector3.Dot(b, b)) * b;

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
