using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardStab : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



    }
    public void DownwardStabFunction(float forceAmount, Rigidbody rigidbody)
    {
        //Debug.Log("hi");
        //add force downwards
        rigidbody.AddForce(Vector3.down * forceAmount);
    }
}
