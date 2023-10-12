using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStateManager : MonoBehaviour
{
    public SwordBaseState currentState;
    public SwordIdleState idleState = new SwordIdleState();
    public SwordThirstyState thirstyState = new SwordThirstyState();

    // Start is called before the first frame update
    void Start()
    {
        // starting state for the state machine
        currentState = idleState;
        // "this" is a ref to the context (this EXACT monobehavior script)
        currentState.EnterState(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    // Update is called once per frame
    void Update()
    {
        // will call any logic in UpdateState from the current state every frame
        currentState.UpdateState(this);
    }

    public void SwitchState(SwordBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
