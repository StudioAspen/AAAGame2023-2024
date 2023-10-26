using System;
using System.Transactions;
using WUG.BehaviorTreeVisualizer;
using UnityEngine;
using UnityEngine.AI;
using EnemyBehaviorTrees.Agents;
using EnemyBehaviorTrees.Managers;

namespace EnemyBehaviorTrees.Nodes
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
        
        protected override NodeStatus OnRun()
        {
            //Confirm all references exist
            if (EnemyBehaviorTreeGameManager.Instance == null || EnemyBehaviorTreeGameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager or NPC is null";
                return NodeStatus.FAILURE;
            }
        
            // Perform logic that should only run once
            if (EvaluationCount == 0)
            {
                // Get destination from Game Manager 
                GameObject destinationGO = null;
                NavigationActivity currentActivity = EnemyBehaviorTreeGameManager.Instance.NPC.MyActivity;

                switch (currentActivity)
                {
                    case NavigationActivity.LOOK_FOR_PLAYER:
                        destinationGO = EnemyBehaviorTreeGameManager.Instance.GetPlayerWithinRange(EnemyBehaviorTreeGameManager.Instance.NPC.playerAggroRange);
                        break;
                    case NavigationActivity.CHASE:
                        destinationGO = EnemyBehaviorTreeGameManager.Instance.GetPlayerWithinRange(EnemyBehaviorTreeGameManager.Instance.NPC.playerAggroRange);
                        break;
                    case NavigationActivity.PATROL:
                        destinationGO = EnemyBehaviorTreeGameManager.Instance.GetNextWayPoint();
                        break;
                }
        
                // Confirm that the destination is valid - If not, fail.
                if (destinationGO == null)
                {
                    StatusReason = $"Unable to find game object for {EnemyBehaviorTreeGameManager.Instance.NPC.MyActivity}";
                    return NodeStatus.FAILURE;
                }
        
                // Get a valid location on the NavMesh that's near the target destination
                // IDEA SO THAT AGENT FOLLOWS PLAYER MORE ACCURATELY: CHANGE SAMPLE DISTANCE TO HOWEVER MUCH DISTANCE THE AGENT CAN TRAVEL AT THE GIVEN THINKING TIME SO THAT IT CAN COMPLETE
                // MORE OFTEN AND THUS MAKES THE CHASING AND DEAGGRO MORE ACCURATE.
                NavMesh.SamplePosition(destinationGO.transform.position, out NavMeshHit hit, 1f, 1);
                
                // Set the location for checks later
                targetDestination = hit.position;
        
                // Set the destination on the NavMesh. This tells the AI to start moving to the new location.
                EnemyBehaviorTreeGameManager.Instance.NPC.MyNavMesh.SetDestination(targetDestination);
                StatusReason = $"Starting to navigate to {destinationGO.transform.position}";
                
                // Return running, as we want to continue to have this node evaluate
                return NodeStatus.RUNNING;
            }
        
            // Calculate how far the AI is from the destination
            float distanceToTarget = Vector3.Distance(targetDestination, EnemyBehaviorTreeGameManager.Instance.NPC.transform.position);
        
            // If the AI is within .25f then navigation will be considered a success
            if (distanceToTarget < .25f)
            {
                StatusReason = $"Navigation ended. " +
                    $"\n - Evaluation Count: {EvaluationCount}. " +
                    $"\n - Target Destination: {targetDestination}" +
                    $"\n - Distance to target: {Math.Round(distanceToTarget, 1)}";
        
                return NodeStatus.SUCCESS;
            }
        
            // Otherwise, the AI is still on the move
            StatusReason = $"Distance to target: {distanceToTarget}";
            return NodeStatus.RUNNING;
        
        }
    }
}