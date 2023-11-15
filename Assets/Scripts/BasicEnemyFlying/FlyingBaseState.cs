using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlyingBaseState 
{
   public abstract void EnterState(FlyingEnemyManager Enemy);

   public abstract void UpdateState(FlyingEnemyManager Enemy);
}
