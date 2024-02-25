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
    EnergyBlast energyBlast;

    PlayerAction currentAction;

    private void Start() {
        // Getting components
        basicMovementAction = GetComponent<BasicMovementAction>();
        dashAction = GetComponent<DashAction>();
        stabAction = GetComponent<StabAction>();
        slashAction = GetComponent<SlashAction>();
        downwardStabAction = GetComponent<DownwardStabAction>();
        stabDashAction = GetComponent<StabDashAction>();
        slashDashAction = GetComponent<SlashDashAction>();
        energyBlast = GetComponent<EnergyBlast>();

        currentAction = basicMovementAction;
    }

    public void DirectionalInput(Vector3 input) {
        if (currentAction == basicMovementAction) {
            // Since this is continuously getting input, to check when the player is not inputting is checking the magnititute
            if (input.magnitude > 0.01f) {
                basicMovementAction.MoveInput(input);
            }
            else {
                basicMovementAction.NoMoveInput();
            }
        }
    }
    public void JumpInput() {
        if(currentAction == basicMovementAction && basicMovementAction.CanPerformJump()) {
            basicMovementAction.JumpInput();
        }
    }
    public void DashInput(Vector3 input) {
        if (currentAction == basicMovementAction && dashAction.CanPerformDash()) {
            ChangeAction(dashAction);
            dashAction.DashInput(input);
        }
    }
    public void StabInputPressed() {
        if(currentAction == basicMovementAction && stabAction.CanPerformStab()) {
            ChangeAction(stabAction);
            stabAction.StabInput();
        }
    }
    public void StabInputHold() {
        if(currentAction == stabAction) {
            downwardStabAction.DownwardStabInputUpdate();
        }
    }
    public void StabInputRelease() {
        if (currentAction == basicMovementAction ||
            currentAction == downwardStabAction) {
            downwardStabAction.DownwardStabInputRelease();
        }
    }
    public void SlashInput() {
        if(currentAction == basicMovementAction && slashAction.CanPerformSlash()) {
            ChangeAction(slashAction);
            slashAction.SlashInput();
        }
    }
    public void EnergyBlastInput() {
        if(currentAction == basicMovementAction) {
            energyBlast.Shoot();
        }
    }

    public void EndAction() {
        currentAction.OnEndAction.RemoveListener(EndAction);
        currentAction = basicMovementAction;
    }
    public void ChangeAction(PlayerAction action) {
        currentAction = action;
        currentAction.OnEndAction.AddListener(EndAction);
    }
}
