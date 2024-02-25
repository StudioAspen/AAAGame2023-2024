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

    [Header("Control Type")]
    [SerializeField] ControlType currentControls;

    [Header("Input Variables")]
    [SerializeField] float combinationWindow;

    [Header("Mouse Keyboard Inputs")]
    [SerializeField] KeyCode keyboardStab;
    [SerializeField] KeyCode keyboardSlash;
    [SerializeField] KeyCode keyboardDash;
    [SerializeField] KeyCode keyboardJump;
    [SerializeField] KeyCode keyboardShoot;

    [Header("Controller Inputs")]
    [SerializeField] KeyCode controllerStab;
    [SerializeField] KeyCode controllerSlash;
    [SerializeField] KeyCode controllerDash;
    [SerializeField] KeyCode controllerJump;
    [SerializeField] KeyCode controllerShoot;

    // Controls
    KeyCode inputStab;
    KeyCode inputSlash;
    KeyCode inputDash;
    KeyCode inputJump;
    KeyCode inputShoot;

    bool canInput = true;

    InputActionCheck inputActionCheck;
    // Start is called before the first frame update
    void Start() {
        
        // Getting camera components
        cameraOrientation = FindObjectOfType<Camera>().transform;
        cinemachineCam = FindObjectOfType<CinemachineFreeLook>();
        inputActionCheck = GetComponent<InputActionCheck>();

        SetCurrentController();
    }

    // Update is called once per frame
    void Update() {
        // Initalizing input direction
        Vector3 inputDirection = Vector3.zero;
        if (canInput) {
            //player input direction is calculated by multiplying forward and right by the horizontal and vertical axes
            inputDirection = cameraOrientation.right * Input.GetAxis("Horizontal") + cameraOrientation.forward * Input.GetAxis("Vertical");
            CheckAbilities(inputDirection);
        }
    }
    public void DisableInput() {
        canInput = false;
    }
    public void EnableInput() {
        canInput = true;
    }
    private void CheckAbilities(Vector3 direction) {
        if (Input.GetKeyDown(inputJump)) {
            inputActionCheck.JumpInput();
        }
        if (Input.GetKeyDown(inputDash)) {
            inputActionCheck.DashInput(direction);
        }
        if (Input.GetKey(inputStab)) {
            inputActionCheck.StabInputHold();
        }
        if (Input.GetKeyUp(inputStab)) {
            inputActionCheck.StabInputRelease();
        }
        // Stab Move with combination logic
        if (Input.GetKeyDown(inputStab)) {
            inputActionCheck.StabInputPressed();
        }
        if (Input.GetKeyDown(inputSlash)) {
            inputActionCheck.SlashInput();
        }
        if (Input.GetKeyDown(inputShoot)) {
            inputActionCheck.EnergyBlastInput();
        }
        inputActionCheck.DirectionalInput(direction);
    }

    private void SetCurrentController() {
        // Setting controls for camera
        switch (currentControls) {
            case ControlType.controller:
                // Setting axis for controller
                cinemachineCam.m_XAxis.m_InputAxisName = "Right Stick Horizontal";
                cinemachineCam.m_YAxis.m_InputAxisName = "Right Stick Vertical";


                // Controller keycode enums are offset when they are set in the editor this is to correct them (controller inputs here)
                controllerStab += 4;
                controllerSlash += 4;
                controllerDash += 4;
                controllerJump += 4;
                controllerShoot += 4;

                // Setting inputs for controller
                inputStab = (controllerStab);
                inputSlash = (controllerSlash);
                inputDash = (controllerDash);
                inputJump = (controllerJump);
                inputShoot = (controllerShoot);
                Debug.Log(inputJump);
                break;
            case ControlType.mouseAndKeyboard:
                // Setting axis for keyboard
                cinemachineCam.m_XAxis.m_InputAxisName = "Mouse X";
                cinemachineCam.m_YAxis.m_InputAxisName = "Mouse Y";
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                keyboardShoot += 7; // Keycode enums are offset when set in the editor (mouse inputs here)

                // Setting inputs for keyboard
                inputStab = keyboardStab;
                inputSlash = keyboardSlash;
                //inputDownwardStab = keyboardDownwardStab;
                inputDash = keyboardDash;
                inputJump = keyboardJump;


                inputShoot = keyboardShoot;

                break;
            default:
                break;
        }
    }
}
