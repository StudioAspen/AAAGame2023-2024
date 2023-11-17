using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Death State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Death Update");
    }
}
