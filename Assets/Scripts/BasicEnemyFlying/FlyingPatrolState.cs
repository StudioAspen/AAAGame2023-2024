using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPatrolState : FlyingBaseState
{
    public override void EnterState(FlyingEnemyManager Enemy)
    {
        Debug.Log("Patrol State");
    }

    public override void UpdateState(FlyingEnemyManager Enemy)
    {
        
    }
}
