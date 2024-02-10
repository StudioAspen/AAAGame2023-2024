using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class FlyingChaseState : FlyingBaseState
{
    public Vector3 direction;
    public Vector3 raycastOrigin;
    public Vector3 downRaycast;
    
    public override void EnterState(FlyingEnemyManager Enemy)
    {
        Debug.Log("Chase State");
      
    }

    public override void UpdateState(FlyingEnemyManager Enemy)
    {
        RaycastHit hit;
        RaycastHit up;
        RaycastHit down;
      

        // Add a variable to control the speed of the raycast movement
        float raycastSpeed = 1.0f;


        
        Enemy.agent.SetDestination(Enemy.playerPosition.transform.position);
        if (Physics.Raycast(Enemy.childFlying.transform.position, Enemy.childFlying.transform.forward, out hit, Enemy.distanceCheck, Enemy.obstacleLayer))
        {
            //Upward Raycast Check
            raycastOrigin += Enemy.childFlying.transform.position + Vector3.up * raycastSpeed * Time.deltaTime;
            Physics.Raycast(raycastOrigin, Enemy.childFlying.transform.forward, out up, Enemy.distanceCheck, Enemy.obstacleLayer);
            //Downward Raycast Check
            downRaycast += Enemy.childFlying.transform.position + Vector3.down * raycastSpeed * Time.deltaTime;
            Physics.Raycast(downRaycast, Enemy.childFlying.transform.forward, out down, Enemy.distanceCheck, Enemy.obstacleLayer);



            if (up.collider == null)
            {
                direction = Vector3.up;
               
            }
            else if (down.collider == null)
            {
                direction = Vector3.down;
            }
           
        }
        if(hit.collider == null)
        {
            direction = Vector3.zero;
            raycastOrigin = Vector3.zero;
            downRaycast = Vector3.zero; 
        }
        Debug.Log(direction);
      
        Debug.DrawRay(raycastOrigin, Enemy.childFlying.transform.forward * Enemy.distanceCheck, Color.yellow);

        Enemy.childFlying.transform.Translate(direction * Time.deltaTime * 2f);


     
    }




}

   
