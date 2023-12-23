using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMovementState {
    IDLE,
    STABBING,
    SLASHING,
    DOWNWARD_STAB,
    DASHING,
    WALL_SLIDING,
    STAB_DASHING,
    SLASH_DASHING
}

public class PlayerMovementStateManager : MonoBehaviour
{
    public PlayerMovementState currentState = PlayerMovementState.IDLE;

    public void ChangeState(PlayerMovementState newState) {
        currentState = newState;
    }
}
