using UnityEditor.iOS.Extensions.Common;
using UnityEngine;

// default idle state of sword
public class SwordIdleState : SwordBaseState
{
    public override void EnterState(SwordStateManager sword)
    {
        // give control to player to use
    }

    public override void UpdateState(SwordStateManager sword)
    {
        // attack, stab, parry, etc
        // sword gauge is below 10% go into thirsty state
        sword.SwitchState(sword.thirstyState);
    }

    public override void OnCollisionEnter(SwordStateManager sword, Collision collision)
    {
        // if tag == enemy do damage
    }
}
