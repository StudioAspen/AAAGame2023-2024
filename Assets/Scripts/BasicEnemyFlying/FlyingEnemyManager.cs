using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class FlyingEnemyManager : MonoBehaviour
{

    FlyingBaseState currentState;
    public FlyingChaseState chaseState = new FlyingChaseState();
    public FlyingPatrolState patrolState = new FlyingPatrolState();


    public NavMeshAgent agent;
    public Transform playerPosition;

    public GameObject childFlying;
    public float distanceCheck;
    public float aggroDistance;

    public float evasionSpeed;

     public Vector3 pathRotate = Vector3.zero;
    [SerializeField] public LayerMask obstacleLayer;
    // Start is called before the first frame update
    void Start()
    {
        currentState = chaseState;
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != null)
        {
            currentState.UpdateState(this);
        }
      
    }

    public void switchState(FlyingBaseState newState)
    {
            currentState = newState;
            currentState.EnterState(this);
    }
    public bool RayCastCheck(float distance)
    {
        if (Physics.Raycast(transform.position, (playerPosition.transform.position - transform.position), out RaycastHit hitInfo, distance))
        {
            return true;
        }
        else
        { 
            return false;
        }
    }
  
}
