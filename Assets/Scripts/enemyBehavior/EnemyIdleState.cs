using UnityEngine;

// default idle state of enemy
public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Idle State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Idle Update");
    }
}
