using System;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;
using EnemyBehaviorTrees.Agents;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyBehaviorTrees.Actions
{
    // Action node for an agent to navigate to a certain destination.
    
    
    public class NavigateToDestination : Node
    {
        private Vector3 targetDestination;

        public NavigateToDestination()
        {
            Name = "Navigate";
        }

        // OnReset() - empty
        protected override void OnReset() { }
        
        protected override NODE_STATUS OnRun()
        {
            //Confirm all references exist
            if (BehaviorTreeTestGameManager.Instance == null || BehaviorTreeTestGameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager or NPC is null";
                return NODE_STATUS.Failure;
            }
        
            //Perform logic that should only run once
            if (EvaluationCount == 0)
            {
                //Get destination from Game Manager 
                GameObject destinationGO = BehaviorTreeTestGameManager.Instance.NPC.MyActivity == 
                                           NAVIGATION_ACTIVITY.PickupItem ?  BehaviorTreeTestGameManager.Instance.GetClosestItem() 
                    : BehaviorTreeTestGameManager.Instance.GetNextWayPoint();
        
                //Confirm that the destination is valid - If not, fail.
                if (destinationGO == null)
                {
                    StatusReason = $"Unable to find game object for {BehaviorTreeTestGameManager.Instance.NPC.MyActivity}";
                    return NODE_STATUS.Failure;
                }
        
                //Get a valid location on the NavMesh that's near the target destination
                NavMesh.SamplePosition(destinationGO.transform.position, out NavMeshHit hit, 1f, 1);
                
                //Set the location for checks later
                targetDestination = hit.position;
        
                //Set the destination on the NavMesh. This tells the AI to start moving to the new location.
                BehaviorTreeTestGameManager.Instance.NPC.MyNavMesh.SetDestination(targetDestination);
                StatusReason = $"Starting to navigate to {destinationGO.transform.position}";
                
                //Return running, as we want to continue to have this node evaluate
                return NODE_STATUS.Running;
            }
        
            //Calculate how far the AI is from the destination
            float distanceToTarget = Vector3.Distance(targetDestination, BehaviorTreeTestGameManager.Instance.NPC.transform.position);
        
            //If the AI is within .25f then navigation will be considered a success
            if (distanceToTarget < .25f)
            {
                StatusReason = $"Navigation ended. " +
                    $"\n - Evaluation Count: {EvaluationCount}. " +
                    $"\n - Target Destination: {targetDestination}" +
                    $"\n - Distance to target: {Math.Round(distanceToTarget, 1)}";
        
                return NODE_STATUS.Success;
            }
        
            //Otherwise, the AI is still on the move
            StatusReason = $"Distance to target: {distanceToTarget}";
            return NODE_STATUS.Running;
        
        }
    }
}