using UnityEngine;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;
using EnemyBehaviorTrees.Managers;

namespace EnemyBehaviorTrees.Nodes
{
    // This is a condition that tests whether an NPC agent's current NavigationActivity is a certain NavigationActivity to check for.
    // NOTE: This is a test condition from https://gamedev-resources.com/get-started-with-behavior-trees/ #Condition section.

    public class IsPlayerNearBy : Condition
    {
        private float distanceToCheck;

        // Constructor - just changes the name of the base node object to be descriptive of the distance it checks around the agent
        public IsPlayerNearBy(float maxDistance) : base($"Is player within {maxDistance}f?")
        {
            distanceToCheck = maxDistance;
        }

        // OnReset() - empty
        protected override void OnReset() {}

        protected override NodeStatus OnRun()
        {
            // Check for references
            if (EnemyBehaviorTreeGameManager.Instance == null || EnemyBehaviorTreeGameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager and/or NPC is null";
                return NodeStatus.FAILURE;
            }

            // Check if the player is nearby
            GameObject player = EnemyBehaviorTreeGameManager.Instance.GetPlayerWithinRange(distanceToCheck);

            // Check to see if player was returned
            if (player == null)
            {
                StatusReason = $"Player is not within {distanceToCheck} units";
                return NodeStatus.FAILURE;
            }

            // Else, player is within range, return Success
            return NodeStatus.SUCCESS;
        }
    }
}