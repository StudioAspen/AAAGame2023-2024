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
            // "once the player character is out of view they will move at a slow jog from their current position to the player character’s last seen location"
            // run to player's last position, then chill for 5 seconds
            enemy.SwitchToIdle();
        }
        // if enemy is in range for attack
        else if (enemy.RayCastCheck(2f))
            enemy.SwitchState(enemy.attackState);

        // moves towards player until in/out of range
        enemy.MoveTowardsPlayer();
    }
}
