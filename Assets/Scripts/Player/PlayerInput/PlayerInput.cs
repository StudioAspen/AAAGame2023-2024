using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
public class PlayerInput : MonoBehaviour {
    // Components
    enum ControlType { controller, mouseAndKeyboard };
    CinemachineFreeLook cinemachineCam;
    Transform cameraOrientation;
    Animator playerAni;

    [Header("Control Type")]
    [SerializeField] ControlType currentControls;

    [Header("Input Variables")]
    [SerializeField] float combinationWindow;

    [Header("Mouse Keyboard Inputs")]
    [SerializeField] KeyCode keyboardStab;
    [SerializeField] KeyCode keyboardSlash;
    //[SerializeField] KeyCode keyboardDownwardStab;
    [SerializeField] KeyCode keyboardDash;
    [SerializeField] KeyCode keyboardJump;

    [Header("Controller Inputs")]
    [SerializeField] KeyCode controllerStab;
    [SerializeField] KeyCode controllerSlash;
    //[SerializeField] KeyCode controllerDownwardStab;
    [SerializeField] KeyCode controllerDash;
    [SerializeField] KeyCode controllerJump;

    // Controls
    KeyCode inputStab;
    KeyCode inputSlash;
    //KeyCode inputDownwardStab;
    KeyCode inputDash;
    KeyCode inputJump;

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
        playerAni = GetComponent<Animator>();
        
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
                // Setting axis for controller
                cinemachineCam.m_XAxis.m_InputAxisName = "Right Stick Horizontal";
                cinemachineCam.m_YAxis.m_InputAxisName = "Right Stick Vertical";


                controllerStab += 4;
                controllerSlash += 4;
                //controllerDownwardStab += 4;
                controllerDash += 4;
                controllerJump += 4;

                // Setting inputs for controller
                inputStab = (controllerStab);
                inputSlash = (controllerSlash);
                //inputDownwardStab = (controllerDownwardStab); 
                inputDash = (controllerDash);
                inputJump = (controllerJump);
                Debug.Log(inputJump);
                break;
            case ControlType.mouseAndKeyboard:
                // Setting axis for keyboard
                cinemachineCam.m_XAxis.m_InputAxisName = "Mouse X";
                cinemachineCam.m_YAxis.m_InputAxisName = "Mouse Y";
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                // Setting inputs for keyboard
                inputStab = keyboardStab;
                inputSlash = keyboardSlash;
                //inputDownwardStab = keyboardDownwardStab;
                inputDash = keyboardDash;
                inputJump = keyboardJump;
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
            CheckCombinationAbilties(inputDirection);
            CheckAbilities();
        }
        else if (dashStarted || slashStarted || stabStarted) { // Checking inputs for combination abilities
            combinationWindowTimer += Time.deltaTime;
            CheckCombinationAbilties(inputDirection);
        }
        movement.Move(inputDirection);
    }
    public void DisableInput() {
        canInput = false;
    }
    public void EnableInput() {
        canInput = true;
    }
    private void CheckAbilities() {
        if (Input.GetKeyDown(inputJump)) {
            movement.JumpFunction();
        }
        if (Input.GetKey(inputStab)) {
            downwardStab.TryDownwardStabUpdate();
        }
        if (Input.GetKeyUp(inputStab)) {
            downwardStab.ReleaseDownwardStab();
        }
    }
    private void CheckCombinationAbilties(Vector3 direction) {
        // Stab Move with combination logic
        if (Input.GetKeyDown(inputStab)) {
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
        if (Input.GetKeyDown(inputSlash)) {
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
        if (Input.GetKeyDown(inputDash)) {
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
