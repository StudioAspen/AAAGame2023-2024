using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MeleeEnemyStateManager : MonoBehaviour
{
    [Header("Melee Enemy State Duration Variables")]
    public float aggroDistance;
    public float deagroDistance;
    public float attackDistance;
    public float idleAtPlayerLastPositionDuration;
    public float stunDuration;
    public float deleteTimer;
    public Transform spawnpoint;

    [Header("Melee Enemy Variables")]
    public GameObject enemyWeapon;
    public float enemyDamage;
    public float enemyAttackCooldown;
    public float enemyAttackDuration;
    private float enemyCDTimer = 0;

    [Header("Enemy Death Speed Boost Variables")]
    public float deathSpeedIncrease;
    public float deathSpeedDuration;

    [Header("References")]
    private Killable kill;
    [HideInInspector] public Renderer render;
    private NavMeshAgent agent;
    public Timer timer = new Timer();
    private Transform playerTransform;
    private bool canAttack;

    [Header("Animations")]
    public Animator animator;


    // all the states of the enemies
    public MeleeEnemyBaseState currentState;
    public MeleeEnemyIdleState idleState;
    public MeleeEnemyAggroState aggroState;
    public MeleeEnemyAttackState attackState;
    public MeleeEnemyStunState stunState;
    public MeleeEnemyDeathState deathState;


    void Start() {
        idleState = new MeleeEnemyIdleState(aggroDistance);
        aggroState = new MeleeEnemyAggroState(deagroDistance, attackDistance);
        attackState = new MeleeEnemyAttackState(attackDistance);
        stunState = new MeleeEnemyStunState(stunDuration);
        deathState = new MeleeEnemyDeathState();

        if (spawnpoint == null) {
            spawnpoint = transform;
        }

        // setting references
        render = GetComponentInChildren<Renderer>();
        playerTransform = FindObjectOfType<PlayerInput>().transform;
        agent = gameObject.GetComponent<NavMeshAgent>();
        enemyWeapon.SetActive(false);

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

        //
        enemyCDTimer -= Time.deltaTime;
    }

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
    public void SwitchState(MeleeEnemyBaseState state)
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
        render.material.color = Color.red;
        SwitchState(stunState);
    }

    // switch state to deathstate
    public void Death()
    {
        render.material.color = Color.black;
        SwitchState(deathState);
    }
    #endregion

    // adds a speed boost to player when enemy dies
    public void DeleteOnDeath()
    {
        playerTransform.GetComponentInChildren<MovementModification>().AddSpeedBoost(deathSpeedDuration, deathSpeedIncrease);
        Destroy(gameObject);
    }

    // checks if enemy cd 
    public void MeleeAttack()
    {
        if(enemyCDTimer <= 0)
        {
            enemyWeapon.GetComponent<MeleeSword>().hitPlayer = false;
            enemyWeapon.SetActive(true);
            enemyCDTimer = enemyAttackCooldown;
            if (!timer.IsActive())
                timer.StartTimer(enemyAttackDuration, EndMelee);
            Debug.Log("sword active");
        }
    }
    
    // turns the melee sword hitbox off
    public void EndMelee()
    {
        enemyWeapon.GetComponent<MeleeSword>().damage = enemyDamage;
        enemyWeapon.SetActive(false);
        Debug.Log("set false");
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
