using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Aggro State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Aggro Update");
    }
}
