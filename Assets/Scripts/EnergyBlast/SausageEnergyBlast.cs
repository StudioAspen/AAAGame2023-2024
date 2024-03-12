using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageEnergyBlast : EnergyBlastedEffect
{
    [HideInInspector] public bool isStunned;
    private EnemyStateManager enemy;
    private MeleeEnemyStateManager meleeEnemy;

    public override void TriggerEffect()
    {
        // Energy blasts stun this enemy for a few seconds.
        // If the enemy is killed while stunned then the enemy drops an increased amount of blood.
        if(!isStunned)
        {
            isStunned = true;
            if (gameObject.TryGetComponent(out EnemyStateManager range))
                range.Stun();
            if (gameObject.TryGetComponent(out MeleeEnemyStateManager melee))
                melee.Stun();
        }
    }
}
