using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Aggro State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Aggro Update");

        // if enemy is out of deaggro range
        if (!(enemy.RayCastCheck(25f)))
        {
            enemy.SwitchState(enemy.idleState);
        }
        // if enemy is in range for attack
        else if (enemy.RayCastCheck(2f))
        {
            enemy.SwitchState(enemy.attackState);
        }
    }
}
