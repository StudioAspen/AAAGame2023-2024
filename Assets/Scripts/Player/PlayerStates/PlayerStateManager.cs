using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState { 
    IDLE,
    STAB,
    SLASH,
    DASH,
    DOWNWARD_STAB,
    DASH_SLASH,
    DASH_STAB,
    DASH_THROUGH,
    SLIDING,
    STUNNED,
    DEAD
}

public class PlayerStateManager : MonoBehaviour
{
    // States
    //PlayerStateBase currentState;
    public PlayerState currentState;
    PlayerIdleState idleState;

    // Actions
    DashAction dashAction;
    EnergyBlast energyBlast;

    private void Start() {
        currentState = PlayerState.IDLE;
    }
}
