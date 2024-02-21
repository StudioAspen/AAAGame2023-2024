using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public Renderer renderer;

    // all the states of the enemies
    public EnemyBaseState currentState;
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyAggroState aggroState = new EnemyAggroState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyStunState stunState = new EnemyStunState();
    public EnemyDeathState deathState = new EnemyDeathState();

    // player transform
    private Transform playerTransform;
    // enemy killable
    private Killable kill;

    // AI pathfinding 
    public Transform ogSpawn; // need to drag in component of where the enemy originally spawned
    private NavMeshAgent agent;

    // timer class that nelson made
    public Timer timer = new Timer();
    // time for how long the enemy idles for
    public float timeToSwitch;
    // time for how long the enemy stuns for
    public float timeToStun;

    // cd for enemy attack projectile
    public float enemyAttackCD;

    // bullet prefab
    public GameObject bulletPrefab;
    public GameObject bulletFirePoint;
    public float bulletSpeed;
    public float bulletDmg;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        playerTransform = FindObjectOfType<PlayerInput>().transform;

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
        timer.UpdateTimer();
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

    // stops the enemy/vav mesh agent
    public void StopPosition()
    {
        agent.SetDestination(transform.position);
    }

    // switch state to deathstate
    public void Death()
    {
        SwitchState(deathState);
    }

    // switches state to idle
    public void Idle()
    {
        if (gameObject.GetComponent<SausageEnergyBlast>().isStunned)
            gameObject.GetComponent<SausageEnergyBlast>().isStunned = false;
        SwitchState(idleState);
    }

    // switches state to stin
    public void Stun()
    {
        //dont forget to return color bacck
        renderer.material.color = Color.red;
        SwitchState(stunState);
    }

    // after a certain amount of time switch to idle, mimics the deaggro time where they are 
    // standing still
    public void SwitchToIdle()
    {
        if(!timer.IsActive())
            timer.StartTimer(timeToSwitch, Idle);
    }

    // shoots a bullet at player position
    public void MakeBullet()
    {
        Vector3 toPlayer = playerTransform.position - transform.position;
        GameObject currentBullet = Instantiate(bulletPrefab, bulletFirePoint.transform.position, Quaternion.identity);
        currentBullet.GetComponent<Bullet>().moveForce = toPlayer * bulletSpeed;
    }

    // shoots a bullet at player position
    public void ShootBullet()
    {
        if (!timer.IsActive())
            timer.StartTimer(enemyAttackCD, MakeBullet);
    }

    // STUNNED for amount of time before going back to idle
    public void IsStunned()
    {
        if (!timer.IsActive())
            timer.StartTimer(timeToStun, Idle);
    }
}
