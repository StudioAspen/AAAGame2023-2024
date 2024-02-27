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
    [SerializeField] ControlType currentControls; // Current control set up, keyboard or mouse

    [Header("Input Variables")]
    [SerializeField] float combinationWindow; // The window of time you have to press two inputs at the same time to count as a combination move (StabDashAction and SlashDashAction)


    // All of these are inputs for their respective inputs
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

    PlayerActionManager playerActionManager;
    // Start is called before the first frame update
    void Start() {
        
        // Getting camera components
        cameraOrientation = FindObjectOfType<Camera>().transform;
        cinemachineCam = FindObjectOfType<CinemachineFreeLook>();
        playerActionManager = GetComponentInChildren<PlayerActionManager>();
        playerActionManager.combinationWindow = combinationWindow;

        SetCurrentController();
    }

    // Update is called once per frame
    void Update() {
        if (canInput) {
            // Initalizing input direction
            Vector3 inputDirection = Vector3.zero;

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
            playerActionManager.JumpInput();
        }
        if (Input.GetKeyDown(inputDash)) {
            playerActionManager.DashInput(direction);
        }
        if (Input.GetKey(inputStab)) {
            playerActionManager.StabInputHold();
        }
        if (Input.GetKeyUp(inputStab)) {
            playerActionManager.StabInputRelease();
        }
        if (Input.GetKeyDown(inputStab)) {
            playerActionManager.StabInputPressed(direction);
        }
        if (Input.GetKeyDown(inputSlash)) {
            playerActionManager.SlashInput(direction);
        }
        if (Input.GetKeyDown(inputShoot)) {
            playerActionManager.EnergyBlastInput();
        }
        playerActionManager.DirectionalInput(direction);
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
