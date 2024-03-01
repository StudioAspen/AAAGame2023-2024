using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    float attackDistance;

    public Animator animator;

    public EnemyAttackState(float _attackDistance) {
        attackDistance = _attackDistance;
    }
    public override void EnterState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Attack State");
        enemy.animator.SetBool("isAttacking", true);

    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Attack Update");

        // if enemy is out of range for attack
        if (!(enemy.RayCastCheck(attackDistance)))
            enemy.SwitchState(enemy.aggroState);
            enemy.animator.SetBool("isAttacking", false);

        // NOT DONE
        // "plants their feet and only rotates from their position to aim and shoot projectiles at the player character"
        // stop moving then rotates to face player and shoot projectile at them
        // projectile travels straight and does some damage
        enemy.StopPosition();
        //Debug.Log("enemy stopped, shoot now");
        enemy.ShootBullet();

    }
}
