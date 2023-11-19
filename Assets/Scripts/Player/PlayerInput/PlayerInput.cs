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

    [Header("Input Variables")]
    [SerializeField] float combinationWindow;
    
    enum ControlType { controller, mouseAndKeyboard};
    CinemachineFreeLook cinemachineCam;
    Transform cameraOrientation;

    bool stabStarted = false;
    bool slashStarted = false;
    bool dashStarted = false;
    float combinationWindowTimer = 0;
    bool canInput = true;

    //Movement abilities
    PlayerMovement movement;
    DashMovement dash;
    Stab stab;
    Slash slash;
    DownwardStab downwardStab;
    StabDash stabDash;

    // Start is called before the first frame update
    void Start() {
        // Getting components
        dash = GetComponent<DashMovement>();
        stab = GetComponent<Stab>();
        slash = GetComponent<Slash>();
        movement = GetComponent<PlayerMovement>();
        downwardStab = GetComponent<DownwardStab>();
        stabDash = GetComponent<StabDash>();
        
        // Getting camera components
        cameraOrientation = FindObjectOfType<Camera>().transform;
        cinemachineCam = FindObjectOfType<CinemachineFreeLook>();

        // Setting ability events
        stab.OnStabEnd.AddListener(ResetCombination);
        slash.OnSlashEnd.AddListener(ResetCombination);
        dash.OnDashEnd.AddListener(ResetCombination);
        stabDash.OnEndStabDash.AddListener(ResetCombination);

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
    void Update() {
        Vector3 direction = Vector3.zero;


        if(dashStarted || slashStarted || stabStarted) {
            combinationWindowTimer += Time.deltaTime;
        }

        if (canInput) {
            //player input direction is calculated by multiplying forward and right by the horizontal and vertical axes
            direction = cameraOrientation.right * Input.GetAxis("Horizontal") + cameraOrientation.forward * Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Space)) {
                movement.JumpFunction();
            }

            //Combat Moves

            // Stab Move with combination logic
            if (Input.GetKeyDown(KeyCode.E)) {
                Debug.Log(combinationWindowTimer);
                if(!dashStarted) {
                    stabStarted = true;
                    combinationWindowTimer = 0;
                    stab.StartStab();
                }
                else if(combinationWindowTimer < combinationWindow) {
                    dash.InterruptDash(true);
                    stabDash.TryStartStabDash(direction);
                }

            }

            // Slash Move with combination logic
            if (Input.GetKeyDown(KeyCode.Q)) {
                if (!dashStarted) {
                    slashStarted = true;
                    combinationWindowTimer = 0;
                    slash.StartSlash();
                }
                else if(combinationWindowTimer < combinationWindow) {
                    // slashDash.TryStartStabDash(direction);
                }
            }

            // Dash with combination logic
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                Debug.Log(combinationWindowTimer);
                if (stabStarted && combinationWindowTimer < combinationWindow) {
                    stab.InterruptStab();
                    stabDash.TryStartStabDash(direction);
                }
                else if(slashStarted && combinationWindowTimer < combinationWindow) {
                    slash.InterruptSlide();
                    // slashDash
                }
                else {
                    dashStarted = true;
                    combinationWindowTimer = 0;
                    dash.TryPlayerInputDash(direction);
                }
            }
            if (Input.GetKey(KeyCode.F)) {
                downwardStab.TryDownwardStabUpdate();
            }
            if(Input.GetKeyUp(KeyCode.F)) {
                downwardStab.ReleaseDownwardStab();
            }
            if(Input.GetKeyDown(KeyCode.C)) {
                stabDash.TryStartStabDash(direction);
            }
        }
        movement.Move(direction);
    }
    public void DisableInput() {
        canInput = false;
    }
    public void EnableInput() {
        canInput = true;
    }
    private void ResetCombination() {
        stabStarted = false;
        slashStarted = false;
        dashStarted = false;
    }
}
