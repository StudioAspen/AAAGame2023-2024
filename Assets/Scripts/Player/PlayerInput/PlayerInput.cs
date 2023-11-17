using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// CURRENTLY NONE OF THESE MOVEMENTS EXIST IN THIS BRANCH BUT COMMENTED TO SHOW THE INTENTION
    /// This script controls all the player inputs and calculations for them, like the relative camera position
    /// 
    /// For the gameplay developers replace the comments with your respective code, 
    /// If there are any design concerns they will be addressed in a code review
    /// </summary>

    [Header("Control Type")]
    [SerializeField] ControlType currentControls;
    enum ControlType { controller, mouseAndKeyboard};
    CinemachineFreeLook cinemachineCam;
    Transform cameraOrientation;

    UnityEvent currentMovementEnding;
    bool canInput = true;

    //Movement Orientation
    DashMovement dash;
    Stab stab;
    PlayerMovement movement;

    // Start is called before the first frame update
    void Start() {
        // Getting components
        dash = GetComponent<DashMovement>();
        stab = GetComponent<Stab>();
        movement = GetComponent<PlayerMovement>();
        
        // Getting camera components
        cameraOrientation = FindObjectOfType<Camera>().transform;
        cinemachineCam = FindObjectOfType<CinemachineFreeLook>();

        // Setting controls for camera
        switch (currentControls)
        {
            case ControlType.controller:
                cinemachineCam.m_XAxis.m_InputAxisName = "Right Stick Horizontal";
                cinemachineCam.m_YAxis.m_InputAxisName = "Right Stick Vertical";

                break;
            case ControlType.mouseAndKeyboard:
                cinemachineCam.m_XAxis.m_InputAxisName = "Mouse X";
                cinemachineCam.m_YAxis.m_InputAxisName = "Mouse Y";
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            default:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canInput)
        {
            //player input direction is calculated by multiplying forward and right by the horizontal and vertical axes
            Vector3 direction = cameraOrientation.right * Input.GetAxis("Horizontal") + cameraOrientation.forward * Input.GetAxis("Vertical");


            movement.Move(direction);
            if (Input.GetKeyDown(KeyCode.Space)) {
                movement.JumpFunction();
            }



            //Combat Moves
            if (Input.GetKeyDown(KeyCode.E)) {
                //Setting function for ending dash
                stab.StartStab();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("stabbing");
                //slashAndSlide.StartSlash();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                DisableInput();

                //Setting function for ending dash
                dash.OnDashEnd.AddListener(EndingMove);
                currentMovementEnding = dash.OnDashEnd;
                
                dash.PlayerInputDash(direction);
            }
        }
    }

    public void EndingMove()
    {
        EnableInput();
        currentMovementEnding.RemoveListener(EndingMove);
    }
    public void DisableInput() {
        canInput = false;
    }
    public void EnableInput() {
        canInput = true;
    }
}
