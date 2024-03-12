using UnityEngine;

// abstract class for the different enemy states to copy/derive from when in those states
// i.e when player was in range or not in range
public abstract class MeleeEnemyBaseState
{
    // player transform
    public Transform player;

    public abstract void EnterState(MeleeEnemyStateManager enemy);

    public abstract void UpdateState(MeleeEnemyStateManager enemy);

    // public abstract void ExitState(MeleeEnemyStateManager enemy);
}
