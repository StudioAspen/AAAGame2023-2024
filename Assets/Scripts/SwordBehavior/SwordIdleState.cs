using UnityEngine;

// default idle state of sword
public class SwordIdleState : SwordBaseState
{
    public override void EnterState(SwordStateManager sword)
    {
        Debug.Log("Enter Idle State");
    }

    public override void UpdateState(SwordStateManager sword)
    {
        Debug.Log("Enter Idle Update");

        // if gague under a threshhold go into thirsty state
        if (sword.bloodGauge.currentBlood < sword.bloodGauge.bloodThirstThreshold)
            sword.SwitchState(sword.thirstyState);
    }

    public override void OnCollisionEnter(SwordStateManager sword, Collision collision)
    {
        Debug.Log("Idle State Collider");
    }
}
