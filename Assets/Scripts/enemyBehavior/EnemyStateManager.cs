using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    // all the states of the enemies
    public EnemyBaseState currentState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyAggroState aggroState = new EnemyAggroState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyDeathState deathState = new EnemyDeathState();

    // player transform
    public Transform playerTransform; // need to drag in component
    // enemy killable
    private Killable kill;

    // AI pathfinding 
    public Transform ogSpawn; // need to drag in component of where the enemy originally spawned
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        // get component and when enemy dies, switch the state
        kill = GetComponent<Killable>();
        kill.OnDie.AddListener(Death);

        // starting state for the state machine, aka idle
        currentState = idleState;
        // "this" is a ref to the context (this EXACT monobehavior script)
        // will call logic from EnterState
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {    
        // will call any logic in UpdateState from the current state every frame
        currentState.UpdateState(this);

        // check if death state is working
        if(Input.GetKeyDown(KeyCode.K))
        {
            kill.TakeDamage(132);
        }
    }

    // changes the state of the enemy
    public void SwitchState(EnemyBaseState state)
    {
        // change the state then call the EnterState from the new state
        currentState = state;
        state.EnterState(this);
    }

    // checks if ray is hitting at a given distance and returns a bool because of it
    public bool RayCastCheck(float distance)
    {
        if (Physics.Raycast(transform.position, (playerTransform.transform.position - transform.position), out RaycastHit hitInfo, distance))
        {
            // draws the ray in scene when hit, RED
            Debug.DrawRay(transform.position, (playerTransform.transform.position - transform.position) * hitInfo.distance, Color.red);
            return true;
        }
        else
        {
            // draws the ray in scene when NOT hit, GREEN
            Debug.DrawRay(transform.position, (playerTransform.transform.position - transform.position) * distance, Color.green);
            return false;
        }
    }

    // move towards the player
    public void MoveTowardsPlayer()
    {
        agent.SetDestination(playerTransform.position);
    }

    // move to the original position of enemy
    public void MoveOriginalPosition()
    {
        agent.SetDestination(ogSpawn.position);
    }

    // switch state to deathstate
    public void Death()
    {
        SwitchState(deathState);
    }
}
