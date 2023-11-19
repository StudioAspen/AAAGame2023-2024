using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
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
    bool canMove = true;

    //Movement abilities
    PlayerMovement movement;
    DashMovement dash;
    Stab stab;
    Slash slash;
    DownwardStab downwardStab;
    StabDash stabDash;
    SlashDash slashDash;

    // Start is called before the first frame update
    void Start() {
        // Getting components
        dash = GetComponent<DashMovement>();
        stab = GetComponent<Stab>();
        slash = GetComponent<Slash>();
        movement = GetComponent<PlayerMovement>();
        downwardStab = GetComponent<DownwardStab>();
        stabDash = GetComponent<StabDash>();
        slashDash = GetComponent<SlashDash>();
        
        // Getting camera components
        cameraOrientation = FindObjectOfType<Camera>().transform;
        cinemachineCam = FindObjectOfType<CinemachineFreeLook>();

        // Setting ability events
        stab.OnStabEnd.AddListener(ResetCombination);
        slash.OnSlashEnd.AddListener(ResetCombination);
        dash.OnDashEnd.AddListener(ResetCombination);
        stabDash.OnEndStabDash.AddListener(ResetCombination);
        slashDash.OnEndSlashDash.AddListener(ResetCombination);

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
        // Initalizing input direction
        Vector3 inputDirection = Vector3.zero;
        if (canInput) {
            //player input direction is calculated by multiplying forward and right by the horizontal and vertical axes
            inputDirection = cameraOrientation.right * Input.GetAxis("Horizontal") + cameraOrientation.forward * Input.GetAxis("Vertical");

            if(Input.GetKeyDown(KeyCode.P)) {
                stabDash.TryStartStabDash(inputDirection);
            }

            CheckCombinationAbilties(inputDirection);
            CheckAbilities();
        }
        else if (dashStarted || slashStarted || stabStarted) { // Checking inputs for combination abilities
            combinationWindowTimer += Time.deltaTime;
            CheckCombinationAbilties(inputDirection);
        }
        movement.Move(inputDirection);
        JoyStickCheck();
    }
    public void DisableInput() {
        canInput = false;
    }
    public void EnableInput() {
        canInput = true;
    }
    private void JoyStickCheck() {
        if(Input.GetKeyDown(KeyCode.Joystick1Button0)) {
            Debug.Log("joystick0");
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button1)) {
            Debug.Log("joystick1");
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button2)) {
            Debug.Log("joystick2");
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button3)) {
            Debug.Log("joystick3");
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button4)) {
            Debug.Log("joystick4");
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button5)) {
            Debug.Log("joystick5");
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button6)) {
            Debug.Log("joystick6");
        }
    }
    private void CheckAbilities() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            movement.JumpFunction();
        }
        if (Input.GetKey(KeyCode.F)) {
            downwardStab.TryDownwardStabUpdate();
        }
        if (Input.GetKeyUp(KeyCode.F)) {
            downwardStab.ReleaseDownwardStab();
        }
    }
    private void CheckCombinationAbilties(Vector3 direction) {
        // Stab Move with combination logic
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log(combinationWindowTimer);
            if (!dashStarted) {
                stabStarted = true;
                combinationWindowTimer = 0;
                stab.StartStab();
            }
            else if (combinationWindowTimer < combinationWindow) {
                dash.InterruptDash(true);
                //ResetCombination();
                stabDash.TryStartStabDash(direction);
            }
        }

        // Slash Move with combination logic
        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log(combinationWindowTimer);
            if (!dashStarted) {
                slashStarted = true;
                combinationWindowTimer = 0;
                slash.StartSlash();
            }
            else if (combinationWindowTimer < combinationWindow) {
                dash.InterruptDash(true);
                //ResetCombination();
                slashDash.TryStartSlashDash(direction);
            }
        }

        // Dash with combination logic
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Debug.Log(combinationWindowTimer);
            if (stabStarted && combinationWindowTimer < combinationWindow) {
                stab.InterruptStab();
                //ResetCombination();
                stabDash.TryStartStabDash(direction);
            }
            else if (slashStarted && combinationWindowTimer < combinationWindow) {
                slash.InterruptSlash();
                //ResetCombination();
                slashDash.TryStartSlashDash(direction);
            }
            else {
                dashStarted = true;
                combinationWindowTimer = 0;
                dash.TryPlayerInputDash(direction);
            }
        }
    }
    private void ResetCombination() {
        stabStarted = false;
        slashStarted = false;
        dashStarted = false;
    }
}
