using UnityEngine;

// abstract class for the different enemy states to copy/derive from when in those states
// i.e when player was in range or not in range
public abstract class EnemyBaseState
{
    // player transform
    public Transform player;

    public abstract void EnterState(EnemyStateManager enemy);

    public abstract void UpdateState(EnemyStateManager enemy);
}
