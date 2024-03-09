using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombDemon : MonoBehaviour
{
    public float aggroRange;
    public float attackRange;

    public float zigzagSpeed;
    public float zigzagMagnitude;

    public float updateNavFrequencyPerSecond;

    public enum State { idle, attacking, exploding };
    public State state;

    private GameObject player;
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;
    private NavMeshPath path;

    private float updateNavDelay;
    private float elapsed = 0f;

    private void Start()
    {
        state = State.idle;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
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
                Exploding();
                break;
        }
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
                Vector3 a = transform.position - player.transform.position;
                Vector3 b = Vector3.up;
                Vector3 c = Vector3.Cross(a, b).normalized;

                float magnitude = Mathf.Sin(Time.time * zigzagSpeed) * zigzagMagnitude;
                c *= magnitude;

                Vector3 result = player.transform.position + c;
                navMeshAgent.SetDestination(result);

                if (NavMesh.CalculatePath(transform.position, player.transform.position, navMeshAgent.areaMask, path))
                {
                    float dist = Vector3.Distance(transform.position, path.corners[0]);
                    for (int i = 1; i < path.corners.Length; i++)
                    {
                        dist += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                    }

                    if (dist < attackRange)
                    {
                        state = State.exploding;
                        Jump(player.transform);
                    }
                }
            }
        }
    }

    private void Exploding()
    {
        
    }

    private void Jump(Transform target)
    {
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.isStopped = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 toTarget = target.position - transform.position;
        float gSquared = Physics.gravity.sqrMagnitude;
        float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));
        Vector3 velocity = toTarget / T_lowEnergy - Physics.gravity * T_lowEnergy / 2f;
        rb.AddForce(velocity, ForceMode.VelocityChange);
    }
}
