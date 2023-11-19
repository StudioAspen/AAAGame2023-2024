using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Attack State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Attack Update");

        // if enemy is out of range for attack
        if (!(enemy.RayCastCheck(2f)))
            enemy.SwitchState(enemy.aggroState);

        // "plants their feet and only rotates from their position to aim and shoot projectiles at the player character"
        // stop moving then rotates to face player and shoot projectile at them
        // projectile travels straight and does some damage
        // UNDONE
    }
}
