using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunState : EnemyBaseState
{

    public Animator animator;

    public override void EnterState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Stun State");
        enemy.animator.SetBool("isStunned", true);

    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Stun Update");
        // no longer moving
        enemy.StopPosition();
        Debug.Log("stunned");
        // after amount of time no longer stunned
        enemy.IsStunned();
        enemy.animator.SetBool("isStunned", false);

    }
}
