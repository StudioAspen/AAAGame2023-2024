using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stab_and_Dash_Controller : MonoBehaviour
{
    //Define Required Values
    public GameObject sword;

    private enum State { idling, stabbing, dashing };

    


    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
