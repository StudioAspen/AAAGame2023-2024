using UnityEngine;

// when sword is thirsty and not fed
public class SwordThirstyState : SwordBaseState
{
    public override void EnterState(SwordStateManager sword)
    {
        // player loses control over sword
    }

    public override void UpdateState(SwordStateManager sword)
    {
        // now flails around character, could also hit character?
        // drains health overtime
        // if gauge over 10% go into idle state
        sword.SwitchState(sword.idleState);
    }

    public override void OnCollisionEnter(SwordStateManager sword, Collision collision)
    {
        // if tag == enemy do damage
    }
}
