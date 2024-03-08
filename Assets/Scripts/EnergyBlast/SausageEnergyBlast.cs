using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageEnergyBlast : EnergyBlastedEffect
{
    [HideInInspector] public bool isStunned;
    private EnemyStateManager enemy;
    private MeleeEnemyStateManager meleeEnemy;

    private void Start()
    {
        enemy = gameObject.GetComponent<EnemyStateManager>();
        meleeEnemy = gameObject.GetComponent<MeleeEnemyStateManager>();
    }

    public override void TriggerEffect()
    {
        // Energy blasts stun this enemy for a few seconds.
        // If the enemy is killed while stunned then the enemy drops an increased amount of blood.
        if(!isStunned)
        {
            isStunned = true;
            enemy.Stun();
            meleeEnemy.Stun();
        }
    }
}
