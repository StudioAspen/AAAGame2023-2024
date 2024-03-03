using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    //Movement abilities
    BasicMovementAction basicMovementAction;
    DashAction dashAction;
    StabAction stabAction;
    SlashAction slashAction;
    DownwardStabAction downwardStabAction;
    StabDashAction stabDashAction;
    SlashDashAction slashDashAction;
    SlideAction slideAction;
    EnergyBlast energyBlast;

    PlayerAction currentAction;

    public float combinationWindow;

    private void Start() {
        // Getting components
        basicMovementAction = GetComponentInParent<BasicMovementAction>();
        dashAction = GetComponentInParent<DashAction>();
        stabAction = GetComponentInParent<StabAction>();
        slashAction = GetComponentInParent<SlashAction>();
        downwardStabAction = GetComponentInParent<DownwardStabAction>();
        stabDashAction = GetComponentInParent<StabDashAction>();
        slashDashAction = GetComponentInParent<SlashDashAction>();
        slideAction = GetComponentInParent<SlideAction>();
        energyBlast = GetComponentInParent<EnergyBlast>();

        currentAction = basicMovementAction;
    }

    public void DirectionalInput(Vector3 input) {
        if (currentAction == basicMovementAction ||
            currentAction == downwardStabAction ||
            currentAction == stabAction ||
            currentAction == slashAction) {
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
        if(currentAction == slideAction) {
            slideAction.SlideInput(input);
        }
    }
    public void JumpInput() {
        if(currentAction == basicMovementAction) {
            basicMovementAction.JumpInput();
        }
    }
    public void DashInput(Vector3 input) {
        if (currentAction == basicMovementAction && dashAction.CanPerformDash()) {
            ChangeAction(dashAction);
            dashAction.DashInput(input);
        }
        if(currentAction == stabAction && stabAction.timer < combinationWindow) {
            stabAction.EndAction();
            StabDashInput(input);
        }
        if(currentAction == slashAction && slashAction.timer < combinationWindow) {
            slashAction.EndAction();
            SlashDashInput(input);
        }
    }
    public void SlashInput(Vector3 input) {
        if (currentAction == basicMovementAction && slashAction.CanPerformSlash()) {
            ChangeAction(slashAction);
            slashAction.SlashInput();
        }
        if(currentAction == dashAction && dashAction.timer < combinationWindow) {
            dashAction.EndAction();
            SlashDashInput(input);
        }
    }
    public void StabInputPressed(Vector3 input) {
        // Stab Input
        if(currentAction == basicMovementAction && stabAction.CanPerformStab()) {
            ChangeAction(stabAction);
            stabAction.StabInput();
        }
        
        // Stab Dash input
        if(currentAction == dashAction && dashAction.timer < combinationWindow) {
            dashAction.EndAction();
            StabDashInput(input);
        }
    }
    public void StabInputHold() {
        if(currentAction == stabAction) {
            downwardStabAction.DownwardStabInputUpdate();
        }
    }
    public void StabInputRelease() {
        if (currentAction == basicMovementAction ||
            currentAction == downwardStabAction ||
            currentAction == stabAction) {
            downwardStabAction.DownwardStabInputRelease();
        }
    }
    public void StabDashInput(Vector3 input) {
        if (currentAction == basicMovementAction ||
            currentAction == stabDashAction ||
            currentAction == dashAction) {
            ChangeAction(stabDashAction);
            stabDashAction.StabDashInput(input);
        }
    }
    public void SlashDashInput(Vector3 input) {
        if(currentAction == basicMovementAction || 
            currentAction == slashAction || 
            currentAction == dashAction) {
            ChangeAction(slashDashAction);
            slashDashAction.SlashDashInput(input);
        }
    }
    public void EnergyBlastInput() {
        if(currentAction == basicMovementAction ||
            currentAction == slideAction) {
            energyBlast.Shoot();
        }
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
