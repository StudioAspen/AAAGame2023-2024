using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStateManager : MonoBehaviour
{
    public SwordBaseState currentState;
    public SwordIdleState idleState = new SwordIdleState();
    public SwordThirstyState thirstyState = new SwordThirstyState();

    public BloodThirst bloodGauge;

    public float swordDamage;

    void Start()
    {
        // starting state for the state machine, aka idle
        currentState = idleState;
        // "this" is a ref to the context (this EXACT monobehavior script)
        // will call logic from EnterState
        currentState.EnterState(this);
    }

    void Update()
    {
        // will call any logic in UpdateState from the current state every frame
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // when colliding, call OnCollisionEnter from the state
        currentState.OnCollisionEnter(this, collision);
    }

    public void SwitchState(SwordBaseState state)
    {
        // change the state then call the EnterState from the new state
        currentState = state;
        state.EnterState(this);
    }
}
