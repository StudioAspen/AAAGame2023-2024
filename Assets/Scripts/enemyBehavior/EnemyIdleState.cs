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

        // if enemy is in range for aggro
        if(enemy.RayCastCheck(20f))
        {
            enemy.SwitchState(enemy.aggroState);
        }
    }
}
