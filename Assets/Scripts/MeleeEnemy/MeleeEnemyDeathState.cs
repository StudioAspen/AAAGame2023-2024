using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyDeathState : MeleeEnemyBaseState
{

    public Animator animator;
    public float disappearTimer;


    public override void EnterState(MeleeEnemyStateManager enemy)
    {
        enemy.animator.SetBool("isDead", true);
        disappearTimer = enemy.deleteTimer;

        //Debug.Log("Enter Death State");
        // drop a number of hp/blood to player
        // if the enemy is stunned when killed, drop more hp/blood
        // grants temp speed boost to player

        // dropping blood implementation is done in another script
        // same when it is stunned
        // speed boost is done with enemy.DeleteOnDeath();
        //enemy.DeleteOnDeath();
    }

    public override void UpdateState(MeleeEnemyStateManager enemy)
    {
        Debug.Log("Enter Death Update");
        disappearTimer -= Time.deltaTime;
        if(disappearTimer <= 0) {
            Delete(enemy);
        }
    }

    private void Delete(MeleeEnemyStateManager enemy) {
        enemy.DeleteOnDeath();
    }
}
