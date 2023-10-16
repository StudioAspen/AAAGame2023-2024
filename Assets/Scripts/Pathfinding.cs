using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    private Vector3 Target;
    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            Target = Player.transform.position;
            agent.SetDestination(Target);

        }
    }
}
