using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    // Other Components
    Rigidbody rb;

    // Movement abilities
    BasicMovementAction basicMovementAction;
    JumpAction jumpAction;
    DashAction dashAction;
    StabAction stabAction;
    SlashAction slashAction;
    DownwardStabAction downwardStabAction;
    StabDashAction stabDashAction;
    SlashDashAction slashDashAction;
    SlideAction slideAction;
    FlickAction flickAction;
    EnergyBlast energyBlast;

    PlayerAction currentAction;

    public float combinationWindow;

    private void Start() {
        // Getting other Components
        rb = GetComponentInParent<Rigidbody>();

        // Getting components
        basicMovementAction = GetComponentInParent<BasicMovementAction>();
        jumpAction = GetComponentInParent<JumpAction>();
        dashAction = GetComponentInParent<DashAction>();
        stabAction = GetComponentInParent<StabAction>();
        slashAction = GetComponentInParent<SlashAction>();
        downwardStabAction = GetComponentInParent<DownwardStabAction>();
        stabDashAction = GetComponentInParent<StabDashAction>();
        slashDashAction = GetComponentInParent<SlashDashAction>();
        slideAction = GetComponentInParent<SlideAction>();
        flickAction = GetComponentInParent<FlickAction>();
        energyBlast = GetComponentInParent<EnergyBlast>();

        currentAction = basicMovementAction;
    }

    public void DirectionalInput(Vector3 input) {
        // Basic walking around
        if (currentAction == basicMovementAction ||
            currentAction == downwardStabAction ||
            currentAction == stabAction ||
            currentAction == slashAction ||
            currentAction == jumpAction) {
            // Since this is continuously getting input, to check when the player is not inputting is checking the magnititute
            if (input.magnitude > 0.01f) {
                basicMovementAction.MoveInput(input);
            }
            else {
                basicMovementAction.NoMoveInput();
            }
        }
        else {
            basicMovementAction.NoMoveInput();
        }

        // Giving horizontal input for slide for the jump off slide
        if(currentAction == slideAction) {
            slideAction.SlideInput(input);
        }
        if(currentAction == flickAction) {
            flickAction.HorizontalInput(input);
        }
    }
    public void JumpInputPressed() {
        // Normal jump input when on ground
        if(currentAction == basicMovementAction && jumpAction.CanPerformJump()) {
            ChangeAction(jumpAction);
            jumpAction.JumpInputPressed();
        }
        // Interupting slide with jump only if you are sliding
        else if (currentAction == slideAction) {
            slideAction.ApplyHorizontalOffset();
            ChangeAction(jumpAction);
            jumpAction.JumpStart();
        }
        else if(currentAction == flickAction) {
            flickAction.FlickOff();
        }
    }
    public void JumpInputRelease() {
        // Releaseing jump button to cut of velocity
        if(currentAction == jumpAction) {
            jumpAction.JumpInputRelease();
        }
    }
    public void DashInput(Vector3 input) {
        // Regular dash input
        if ((currentAction == basicMovementAction || currentAction == jumpAction) && 
            dashAction.CanPerformDash()) {
            ChangeAction(dashAction);
            dashAction.DashInput(input);
        }

        // Checking input for stab dash action
        if(currentAction == stabAction && stabAction.timer < combinationWindow) {
            stabAction.EndAction();
            StabDashInput(input);
        }

        // checking input for slash dash action
        if(currentAction == slashAction && slashAction.timer < combinationWindow) {
            slashAction.EndAction();
            SlashDashInput(input);
        }
    }
    public void SlashInput(Vector3 input) {
        // Check input for slash
        if ((currentAction == basicMovementAction || currentAction == jumpAction) && 
            slashAction.CanPerformSlash()) {
            ChangeAction(slashAction);
            slashAction.SlashInput();
        }

        // Check input for slash dash
        if(currentAction == dashAction && dashAction.timer < combinationWindow) {
            dashAction.InteruptDash();
            SlashDashInput(input);
        }
    }
    public void StabInputPressed(Vector3 input) {
        // Stab Input
        if((currentAction == basicMovementAction || currentAction == jumpAction) && 
            stabAction.CanPerformStab()) {
            ChangeAction(stabAction);
            stabAction.StabInput();
        }
        
        // Stab Dash input
        if(currentAction == dashAction && dashAction.timer < combinationWindow) {
            dashAction.InteruptDash();
            StabDashInput(input);
        }
    }
    public void StabInputHold() {
        // Holding down stab input for downward stab
        if(currentAction == stabAction) {
            downwardStabAction.DownwardStabInputUpdate();
        }
    }
    public void StabInputRelease() {
        // Release for reseting downward stab
        if (currentAction == basicMovementAction ||
            currentAction == jumpAction ||
            currentAction == downwardStabAction ||
            currentAction == stabAction) {
            downwardStabAction.DownwardStabInputRelease();
        }
    }
    public void StabDashInput(Vector3 input) {
        // Regular input for stabdash
        if (currentAction == basicMovementAction ||
            currentAction == jumpAction ||
            currentAction == stabDashAction ||
            currentAction == dashAction) {
            ChangeAction(stabDashAction);
            stabDashAction.StabDashInput(input);
        }
    }
    public void SlashDashInput(Vector3 input) {
        // Regular input for slash dash
        if (currentAction == basicMovementAction ||
            currentAction == jumpAction ||
            currentAction == slashAction || 
            currentAction == dashAction) {
            ChangeAction(slashDashAction);
            slashDashAction.SlashDashInput(input);
        }
    }
    public void EnergyBlastInput() {
        // Energy blast input
        if (currentAction == basicMovementAction ||
            currentAction == jumpAction ||
            currentAction == slideAction) {
            energyBlast.Shoot();
        }
    }
    
    public void KnockBack(Vector3 source, float launchForce) {
        currentAction.EndAction();
        rb.velocity = ((rb.transform.position-source) + transform.up).normalized * launchForce;
    }

    public void EndOfAction() {
        currentAction.OnEndAction.RemoveListener(EndOfAction);
        currentAction = basicMovementAction;
    }
    public void ChangeAction(PlayerAction action) {
        currentAction.EndAction();
        currentAction = action;
        currentAction.OnEndAction.AddListener(EndOfAction);
    }
}
