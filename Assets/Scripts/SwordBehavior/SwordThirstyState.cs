using UnityEngine;

// when sword is thirsty and not fed
public class SwordThirstyState : SwordBaseState
{
    public override void EnterState(SwordStateManager sword)
    {
        Debug.Log("Enter Thirsty State");
    }

    public override void UpdateState(SwordStateManager sword)
    {
        // Debug.Log("Enter Thirsty Update");

        // if gauge over a threshhold go into idle state
        if (sword.bloodGauge.currentBlood > sword.bloodGauge.bloodThirstThreshold)
            sword.SwitchState(sword.idleState);
    }

    public override void OnCollisionEnter(SwordStateManager sword, Collision collision)
    {
        Debug.Log("Thirsty State Collider");
        
        // make sure collided object has killable component
        if (!collision.gameObject.TryGetComponent<Killable>(out Killable killableComponent)) { Debug.Log("Sword collided object is not killable"); return; }
        
        killableComponent.TakeDamage(sword.swordDamage);
    }
}
