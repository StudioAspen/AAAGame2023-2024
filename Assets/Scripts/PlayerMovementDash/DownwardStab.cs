using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardStab : MonoBehaviour
{
    float stabButtonTimer = 0.0f;
    public float pressDownTime = 0.5f;
    bool shouldStartTimer = false;
    bool canDownwardStab = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //on space (will change to correct input when known) pressed make a timer that will start ticking once pressed and reset on release
        //if timer is more than .5 seconds then do a downward stab


        if (shouldStartTimer)
        {
            //start timer
            stabButtonTimer += Time.deltaTime;
        }else if (!shouldStartTimer)
        {
            //reset timer
            stabButtonTimer = 0f;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldStartTimer = true;
            canDownwardStab = true;

        }else if (Input.GetKeyUp(KeyCode.Space))
        {
            shouldStartTimer = false;

        }else if(stabButtonTimer >= pressDownTime && canDownwardStab)
        {
            //perform the downward stab
            Debug.Log("downward stab initiated");
            canDownwardStab = false;
        }

    }
}
