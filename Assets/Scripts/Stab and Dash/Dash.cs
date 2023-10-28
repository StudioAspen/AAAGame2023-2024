using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Dash : MonoBehaviour
{

    private bool isDashing = false;
    
    private Vector3 dashVelocity = new Vector3(1, 0 , 0);
    public float dashSpeed = 0.5f;
    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called in when sword detects hit
    public void OnTargetHit(SwordScript swordScript)
    {
        dashThroughEntity();
    }



    void dashThroughEntity(/*Collider target*/)
    {
        //NOTE: NEED TO ADD SYSTEM TO DETECT LENGTH OF TRAVEL
        controller.Move(dashVelocity * dashSpeed);

    }
}
