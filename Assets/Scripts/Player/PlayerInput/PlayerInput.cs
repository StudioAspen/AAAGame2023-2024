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
    float combinationWindowTimer = 0;

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

    // Used for input calculations
    enum InputAction {
        NO_INPUT,
        JUMP,
        STAB_PRESS,
        STAB_HOLD,
        STAB_RELEASE,
        SLASH,
        DASH,
        STAB_DASH,
        SLASH_DASH
    }

    InputAction lastInput = InputAction.NO_INPUT;
    List<InputAction> inputsThisFrame = new List<InputAction>();

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
        
        // Getting camera components
        cameraOrientation = FindObjectOfType<Camera>().transform;
        cinemachineCam = FindObjectOfType<CinemachineFreeLook>();

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
        inputsThisFrame.Clear();
        
        if (canInput) {
            //player input direction is calculated by multiplying forward and right by the horizontal and vertical axes
            inputDirection = cameraOrientation.right * Input.GetAxis("Horizontal") + cameraOrientation.forward * Input.GetAxis("Vertical");
            inputsThisFrame = CheckInputs(lastInput); // Checking and calculting the inputs
        }
        if(inputsThisFrame.Count > 0) {
            if(inputsThisFrame[0] != InputAction.STAB_HOLD) {
                lastInput = inputsThisFrame[0];
            }
        }
        ApplyInputs(inputsThisFrame, inputDirection);

        if(Input.GetKeyDown(KeyCode.F)) {
            stabDash.StartStabDash(inputDirection);
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            slashDash.StartSlashDash(inputDirection);
        }

        // Checking inputs for combination abilities
        if (lastInput != InputAction.NO_INPUT) {
            if(combinationWindowTimer > combinationWindow) {
                combinationWindowTimer = 0;
                lastInput = InputAction.NO_INPUT;
            }
            else {
                combinationWindowTimer += Time.deltaTime;
            }
        }
    }
    public void DisableInput() {
        canInput = false;
    }
    public void EnableInput() {
        canInput = true;
    }
    private List<InputAction> CheckInputs(InputAction _lastInput) {
        List<InputAction> inputs = new List<InputAction>();
        
        if (Input.GetKeyDown(inputJump)) {
            inputs.Add(InputAction.JUMP);
        }
        if(Input.GetKey(inputStab)) {
            inputs.Add(InputAction.STAB_HOLD);
        }
        if (Input.GetKeyDown(inputStab)) {
            if (_lastInput == InputAction.DASH) {
                inputs.Add(InputAction.STAB_DASH);
            }
            else {
                inputs.Add(InputAction.STAB_PRESS);
            }
        }
        if (Input.GetKeyUp(inputStab)) {
            inputs.Add(InputAction.STAB_RELEASE);
        }
        // Slash Move with combination logic
        if (Input.GetKeyDown(inputSlash)) {
            if (_lastInput == InputAction.DASH) {
                inputs.Add(InputAction.SLASH_DASH);
            }
            else {
                inputs.Add(InputAction.SLASH);
            }
        }
        // Dash with combination logic
        if (Input.GetKeyDown(inputDash)) {
            if(inputs.Count > 0) {
                if (inputs[inputs.Count-1] == InputAction.STAB_PRESS) {
                    inputs[inputs.Count-1] = InputAction.STAB_DASH;
                }
                else if (inputs[inputs.Count - 1] == InputAction.SLASH) {
                    inputs[inputs.Count - 1] = InputAction.SLASH_DASH;
                }
                else {
                    inputs.Add(InputAction.DASH);
                }
            }
            else {
                inputs.Add(InputAction.DASH);
            }
        }

        return inputs;
    }

    private void ApplyInputs(List<InputAction> inputs, Vector3 direction) {
        foreach (InputAction input in inputs) {
            Debug.Log(input);
            switch (input) {
                case InputAction.JUMP:
                    movement.PlayerInputJump();
                    break;
                case InputAction.STAB_PRESS:
                    stab.StartStab();
                    break;
                case InputAction.STAB_HOLD:
                    downwardStab.DownwardStabUpdate();
                    break;
                case InputAction.STAB_RELEASE:
                    downwardStab.ReleaseDownwardStab();
                    break;
                case InputAction.SLASH:
                    slash.StartSlash();
                    break;
                case InputAction.DASH:
                    dash.PlayerInputDash(direction);
                    break;
                case InputAction.STAB_DASH:
                    stabDash.StartStabDash(direction);
                    break;
                case InputAction.SLASH_DASH:
                    slashDash.StartSlashDash(direction);
                    break;
            }
        }
        movement.Move(direction); // This is an exception because the inputs are important to the implemetation so they are handled in the PlayerMovement Script
    }
}
