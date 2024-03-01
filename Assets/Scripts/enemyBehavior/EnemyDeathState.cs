using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Death State");
        // drop a number of hp/blood to player
        // grants temp speed boost to player
        // if the enemy is stunned when killed, drop more hp/blood
        // UNDONE
        enemy.DeleteOnDeath();
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Death Update");
    }
}
