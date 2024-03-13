using UnityEngine;

// default idle state of enemy
public class MeleeEnemyIdleState : MeleeEnemyBaseState
{
    float aggroDistance;

    public MeleeEnemyIdleState(float _aggroDistance) {
        aggroDistance = _aggroDistance;
    }

    public override void EnterState(MeleeEnemyStateManager enemy)
    {
        // STILL GETS STUNNED USES OLD CODE HOWEVER
        // THERE ARE 2 GETCOMPONENT FOR EACH ENEMY

        //Debug.Log("Enter Idle State");
        // no longer stunned when going back to idle, changes color to signify
        enemy.render.material.color = Color.gray;
        enemy.GetComponent<SausageEnergyBlast>().isStunned = false;
    }

    public override void UpdateState(MeleeEnemyStateManager enemy)
    {
        // starting running to original position
        // if no player stop moving and wait for player to get into range
        // so do nothing, do idle anim
        enemy.MoveOriginalPosition();


        //Debug.Log("Enter Idle Update");
        // if enemy is in range for aggro
        if (enemy.RayCastCheck(aggroDistance))
            enemy.SwitchState(enemy.aggroState);
    }
}
