using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroState : EnemyBaseState
{
    public float deaggroDistance;
    public float attackDistance;

    public EnemyAggroState(float _deaggroDistance, float _attackDistance) {
        deaggroDistance = _deaggroDistance;
        attackDistance = _attackDistance;
    }

    public override void EnterState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Aggro State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Aggro Update");

        // if enemy is out of deaggro range
        if (!(enemy.RayCastCheck(deaggroDistance)))
        {
            // NOT DONE-ish?
            // "once the player character is out of view they will move at a slow jog from their current position to the player character’s last seen location"
            // run to player's last position, then chill for 5 seconds
            // currently idle only goes back to original position
            //enemy.SwitchToIdle(); this is not the intended behavior, you should start this timer when it reaches the LAST SEEN position of the player when theyre out of site - Nelson
            enemy.Idle(); // This is here for march implementation
        }

        // if enemy is in range for attack
        else if (enemy.RayCastCheck(attackDistance))
            enemy.SwitchState(enemy.attackState);

        // moves towards player until in/out of range
        enemy.MoveTowardsPlayer();
    }
}
