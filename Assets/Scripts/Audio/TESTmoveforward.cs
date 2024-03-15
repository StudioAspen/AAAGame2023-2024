using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTmoveforward : MonoBehaviour
{
    [SerializeField]float speed = 5.0f;

    void Update()
    {
        if(Input.GetKey(KeyCode.W)){
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
