using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class BombDemon : MonoBehaviour
{
    public float aggroRange;
    public float attackRange;
    public float updateNavFrequencyPerSecond;

    public enum State {idle, attacking, exploding};
    public State state;

    private GameObject player;
    private BombDemonBody body;
    private NavMeshAgent navMeshAgent;
    private Rigidbody childRigidbody;
    private NavMeshPath path;

    private float updateNavDelay;
    private float elapsed = 0f;

    [Header("JumpAttack")]
    public float explosionRadius;
    public float bloodLoss;
    public float horizontalForce;
    public float verticalForce;
    public bool isAttacking;
    public bool isGrounded;

 
    

    private void Start()
    {
        state = State.idle;
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        body = GetComponentInChildren<BombDemonBody>();
        childRigidbody = GetComponentInChildren<Rigidbody>();
        path = new NavMeshPath();
        updateNavDelay = 1f / updateNavFrequencyPerSecond;
    }

    private void Update()
    {
        switch (state)
        {
            case State.idle:
                Idle();
                break;

            case State.attacking:
                Attacking();
                break;

            case State.exploding:
                Explode();
                break;
        }
        
        if (Input.GetKeyDown(KeyCode.P)) { jumpAttack(body.transform.position, player.transform.position); }
    }

    private void Idle()
    {
        elapsed += Time.deltaTime;
        if (elapsed > updateNavDelay)
        {
            elapsed -= updateNavDelay;
            player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                if (NavMesh.CalculatePath(transform.position, player.transform.position, navMeshAgent.areaMask, path))
                {
                    float dist = Vector3.Distance(transform.position, path.corners[0]);
                    for (int i = 1; i < path.corners.Length; i++)
                    {
                        dist += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                    }

                    if (dist < aggroRange)
                    {
                        navMeshAgent.SetDestination(player.transform.position);
                        state = State.attacking;
                    }
                    
                }
            }
        }

    }

    private void Attacking()
    {
        elapsed += Time.deltaTime;
        if (elapsed > updateNavDelay)
        {
            elapsed -= updateNavDelay;
            player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                navMeshAgent.SetDestination(player.transform.position);
                if (NavMesh.CalculatePath(transform.position, player.transform.position, navMeshAgent.areaMask, path))
                {
                    float dist = Vector3.Distance(transform.position, path.corners[0]);
                    for (int i = 1; i < path.corners.Length; i++)
                    {
                        dist += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                    }

                    if (dist < attackRange)
                    {
                        body.KinematicTurn(false);
                        jumpAttack(body.transform.position, player.transform.position);
                        state = State.exploding;
                    }
                }

            }

        }
    }

    public void jumpAttack(Vector3 objectPosition, Vector3 targetPosition)
    {
        Vector3 playerDirection = (targetPosition - objectPosition).normalized * horizontalForce + Vector3.up * verticalForce;
        childRigidbody.AddForce(playerDirection, ForceMode.Impulse);
        isAttacking = true;

    }

   public bool CollisionDetect(bool _isGrounded)
    {
        isGrounded = _isGrounded;   
        return isGrounded;
    }


    private void Explode()
    {

     if(isAttacking &&  isGrounded)
        {
            body.KinematicTurn(true);
            Collider[] collider = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider hit in collider)
            {
               
             if (hit.TryGetComponent<BloodThirst>(out BloodThirst bloodThirst))
                {
                    bloodThirst.LoseBlood(bloodLoss);
                }

            }
        }
   
    }
  
}
