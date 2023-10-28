using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Stab : MonoBehaviour
{

    //Public Values
    public float stabDistance = 2.0f;
    public float stabTime = 6.0f;


    

    //Define Required Values
    private bool isStabbing = false;
    private Rigidbody rb;
    public GameObject sword;

    //Movement values
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float startTime;
    private float stabSpeed;
    public float speed = 20.0f;
    private bool isMoving = false;

    //Stab Events
    public UnityEvent onStabStart;
    public UnityEvent onStabEnd;
    

    // Start is called before the first frame update
    void Start()
    {

        //Define Distance that the sword will move
        
        targetPosition = sword.transform.position + new Vector3(stabDistance, 0.0f, 0.0f);

        //stabSpeed = stabDistance / stabSpeed;


        

    }

    // Update is called once per frame
    //NOTE: Probably better to use an animator, will figure out later
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isStabbing) // Check for left mouse button click and not already moving.
        {
            targetPosition = sword.transform.position + new Vector3(stabDistance, 0.0f, 0.0f);
            isStabbing = true;
        }

        if (isStabbing)
        {
            float step = speed * Time.deltaTime;
            sword.transform.position = Vector3.MoveTowards(sword.transform.position, targetPosition, step);

            if (sword.transform.position == targetPosition)
            {
                isStabbing = false;
                //Need to add logic for returning sword to original position
            }
        }

    }

    void StartStab()
    {
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / journeyLength;

        transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

        if (fractionOfJourney >= 1.0f)
        {

        }
    }

    public void InterruptStab()
    {
        isStabbing = false;
        //NEED CODE FOR RESETING STAB
    }

    void StabContact()
    {

    }




}
