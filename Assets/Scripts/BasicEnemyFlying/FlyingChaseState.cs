using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingChaseState : FlyingBaseState
{
    public override void EnterState(FlyingEnemyManager Enemy)
    {
        Debug.Log("Chase State");

    }

    public override void UpdateState(FlyingEnemyManager Enemy)
    {
        
    }
}
