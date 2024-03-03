using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract class for the different player states to copy/derive from when in those states
// i.e when player was in range or not in range
public abstract class PlayerStateBase {
    public abstract void EnterState(PlayerStateManager manager);

    public abstract void UpdateState(PlayerStateManager manager);

    public abstract void ExitState(PlayerStateManager manager);
}