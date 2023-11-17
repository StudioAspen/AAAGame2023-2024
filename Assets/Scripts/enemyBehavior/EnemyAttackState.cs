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
    }
}
