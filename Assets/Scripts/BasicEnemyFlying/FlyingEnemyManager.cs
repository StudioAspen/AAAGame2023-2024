using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyManager : MonoBehaviour
{

    FlyingBaseState currentState;
    public FlyingChaseState chaseState = new FlyingChaseState();
    public FlyingPatrolState patrolState = new FlyingPatrolState();
    // Start is called before the first frame update
    void Start()
    {
        currentState = patrolState;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void switchState(FlyingBaseState newState)
    {
            currentState = newState;
            currentState.EnterState(this);
    }
}
