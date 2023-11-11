using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// CURRENTLY NONE OF THESE MOVEMENTS EXIST IN THIS BRANCH BUT COMMENTED TO SHOW THE INTENTION
    /// This script controls all the player inputs and calculations for them, like the relative camera position
    /// 
    /// For the gameplay developers replace the comments with your respective code, 
    /// If there are any design concerns they will be addressed in a code review
    /// </summary>

    bool canMove = true;
    public enum controlType { controller, mouseAndKeyboard};
    public controlType currentControls;
    CinemachineFreeLook cinemachineCam;

    DashMovement dash;
    //stabanddash stabanddash;
    //slashandslide slashandslide;
    PlayerMovement movement;
    public Transform cameraOrientation;

    // Start is called before the first frame update
    void Start()
    {
        cameraOrientation = FindObjectOfType<Camera>().transform;
        dash = GetComponent<DashMovement>();
        cinemachineCam = FindObjectOfType<CinemachineFreeLook>();
        movement = GetComponent<PlayerMovement>();


        switch (currentControls)
        {
            case controlType.controller:
                cinemachineCam.m_XAxis.m_InputAxisName = "Right Stick Horizontal";
                cinemachineCam.m_YAxis.m_InputAxisName = "Right Stick Vertical";

                break;
            case controlType.mouseAndKeyboard:
                cinemachineCam.m_XAxis.m_InputAxisName = "Mouse X";
                cinemachineCam.m_YAxis.m_InputAxisName = "Mouse Y";
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            default:

                break;
        }
        //stabAndDash = GetComponent<StabAndDash>();
        //slashAndSlide = GetComponent<SlashAndSlide>();
        

        //dash.OnStartDash.AddListener(StartingMove);
        //stabAndDash.OnStartStab.AddListener(StartingMove);
        //slashAndSlide.OnStartSlash.AddListener(StartingMove);

        //dash.OnEndDash.AddListener(EndingMove);
        //stabAndDash.OnEndStab.AddListener(EndingMove);
        //slashAndSlide.OnEndSlash.AddListener(EndingMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //player input direction is calculated by multiplying forward and right by the horizontal and vertical axes
            Vector3 direction = cameraOrientation.right * Input.GetAxis("Horizontal") + cameraOrientation.forward * Input.GetAxis("Vertical");

            //Combat Moves
            if (Input.GetKeyDown(KeyCode.E))
            {
                //stabAndDash.StartStab();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //slashAndSlide.StartSlash();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dash.Dash(direction);
            }

            // Regular movement
            if (Input.GetKeyDown(KeyCode.Space))
            {

                movement.JumpFunction();
               
            }
            
                movement.Move(direction);
        }
    }

    public void StartingMove()
    {
        canMove = false;
    }
    public void EndingMove()
    {
        canMove = true;
    }
}
