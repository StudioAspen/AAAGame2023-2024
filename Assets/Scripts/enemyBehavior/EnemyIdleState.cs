using UnityEngine;

// default idle state of enemy
public class EnemyIdleState : EnemyBaseState
{
    float aggroDistance;

    public EnemyIdleState(float _aggroDistance) {
        aggroDistance = _aggroDistance;
    }

    public override void EnterState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Idle State");
        // no longer stunned when going back to idle
        enemy.renderer.material.color = Color.gray;
        enemy.GetComponent<SausageEnergyBlast>().isStunned = false;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        //Debug.Log("Enter Idle Update");

        // if enemy is in range for aggro
        if(enemy.RayCastCheck(aggroDistance))
            enemy.SwitchState(enemy.aggroState);

        // starting running to original position
        // if no player stop moving and wait for player to get into range
        // so do nothing, do idle anim
        enemy.MoveOriginalPosition();
    }
}
