using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyStateManager : MonoBehaviour
{
    [Header("Enemy State Duration Variables")]
    public float aggroDistance;
    public float deagroDistance;
    public float attackDistance;
    public float idleAtPlayerLastPositionDuration;
    public float stunDuration;
    public float deleteTimer;
    public Transform spawnpoint;

    [Header ("Enemy Projectile Variables")]
    public GameObject bulletPrefab;
    public GameObject bulletFirePoint;
    public float bulletSpeed;
    public float bulletDamage;
    public float enemyAttackCooldown;

    [Header("Enemy Death Speed Boost Variables")]
    public float deathSpeedIncrease;
    public float deathSpeedDuration;

    [Header("References")]
    private Killable kill;
    [HideInInspector] public Renderer render;
    private NavMeshAgent agent;
    public Timer timer = new Timer();
    private Transform playerTransform;

    [Header("Animations")]
    public Animator animator;


    // all the states of the enemies
    public EnemyBaseState currentState;
    public EnemyIdleState idleState;
    public EnemyAggroState aggroState;
    public EnemyAttackState attackState;
    public EnemyStunState stunState;
    public EnemyDeathState deathState;


    void Start() {
        idleState = new EnemyIdleState(aggroDistance);
        aggroState = new EnemyAggroState(deagroDistance, attackDistance);
        attackState = new EnemyAttackState(attackDistance);
        stunState = new EnemyStunState(stunDuration);
        deathState = new EnemyDeathState();

        if (spawnpoint == null) {
            spawnpoint = transform;
        }

        // setting references
        render = GetComponentInChildren<Renderer>();
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

    void Update()
    {   
        // will call any logic in UpdateState from the current state every frame
        timer.UpdateTimer();
        currentState.UpdateState(this);
    }

    /*
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
    */

    public bool RayCastCheck(float distance)
    {
        return (playerTransform.position - transform.position).magnitude < distance;
    }

    #region Enemy Movement 

    // move towards the player
    public void MoveTowardsPlayer()
    {
        agent.SetDestination(playerTransform.position);
    }

    // move to the original position of enemy
    public void MoveOriginalPosition()
    {
        agent.SetDestination(spawnpoint.position);
    }

    // stops the enemy/nav mesh agent
    public void StopPosition()
    {
        agent.SetDestination(transform.position);
    }

    #endregion

    #region State Switching

    // changes the state of the enemy
    public void SwitchState(EnemyBaseState state)
    {
        // change the state then call the EnterState from the new state
        currentState = state;
        currentState.EnterState(this);
    }

    // switches state to idle, unstuns enemy if stunned
    public void Idle()
    {
        if (gameObject.GetComponent<SausageEnergyBlast>().isStunned) {
            gameObject.GetComponent<SausageEnergyBlast>().isStunned = false;
        }
        SwitchState(idleState);
    }

    // after a certain amount of time switch to idle, mimics the deaggro time where they are standing still
    public void SwitchToIdle()
    {
        if (!timer.IsActive())
            timer.StartTimer(idleAtPlayerLastPositionDuration, Idle);
    }

    // switches state to stun, change material color to better indicate stun
    // set stun boolean to true
    public void Stun()
    {
        SwitchState(stunState);
    }

    // switch state to deathstate
    public void Death()
    {
        render.material.color = Color.black;
        SwitchState(deathState);
    }
    #endregion

    public void DeleteOnDeath()
    {
        playerTransform.GetComponentInChildren<MovementModification>().AddSpeedBoost(deathSpeedDuration, deathSpeedIncrease);
        Destroy(gameObject);
    }

    // shoots a bullet at player position
    public void MakeBullet()
    {
        Vector3 toPlayer = playerTransform.position - transform.position;
        GameObject currentBullet = Instantiate(bulletPrefab, bulletFirePoint.transform.position, Quaternion.identity);
        // update bullet script with parameters here in manager
        currentBullet.GetComponent<Bullet>().moveForce = toPlayer * bulletSpeed;
        currentBullet.GetComponent<Bullet>().damage = bulletDamage;
    }

    // shoots a bullet at player position
    public void ShootBullet()
    {
        if (!timer.IsActive())
            timer.StartTimer(enemyAttackCooldown, MakeBullet);
    }

    private void OnDrawGizmos() {
        Color holder = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, deagroDistance);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = holder;
    }
}
