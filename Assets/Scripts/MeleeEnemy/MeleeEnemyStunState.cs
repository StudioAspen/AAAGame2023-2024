using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyStunState : MeleeEnemyBaseState
{

    public Animator animator;

    public override void EnterState(MeleeEnemyStateManager enemy)
    {
        //Debug.Log("Enter Stun State");
        enemy.animator.SetBool("isStunned", true);

    }

    public override void UpdateState(MeleeEnemyStateManager enemy)
    {
        //Debug.Log("Enter Stun Update");
        // no longer moving
        enemy.StopPosition();
        //Debug.Log("stunned");
        // after amount of time no longer stunned
        enemy.IsStunned();
        enemy.animator.SetBool("isStunned", false);

    }
}
