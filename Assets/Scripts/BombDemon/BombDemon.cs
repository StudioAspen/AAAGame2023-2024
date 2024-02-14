using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombDemon : MonoBehaviour
{
    public float aggroRange;
    public float updateNavFrequencyPerSecond;

    private enum State {idle, attacking, exploding};
    private State state;

    private GameObject player;
    private NavMeshAgent navMeshAgent;
    private NavMeshPath path;

    private float updateNavDelay;
    private float elapsed = 0f;

    private void Start()
    {
        state = State.idle;
        player = GameObject.FindGameObjectWithTag("Player");
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
            }
        }
    }

    private void Exploding()
    {

    }


}
