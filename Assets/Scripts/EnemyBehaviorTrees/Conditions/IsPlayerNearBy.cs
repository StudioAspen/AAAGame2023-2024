using UnityEngine;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Internal;

namespace EnemyBehaviorTrees.Nodes
{
    // This is a condition that tests whether an NPC agent's current NavigationActivity is a certain NavigationActivity to check for.
    // NOTE: This is a test condition from https://gamedev-resources.com/get-started-with-behavior-trees/ #Condition section.

    public class IsPlayerNearBy : Condition
    {
        // The context is the current NPC agent that is running this node.
        protected NPCAgentBase context { get; }
        private float distanceToCheck;

        // Constructor - just changes the name of the base node object to be descriptive of the distance it checks around the agent
        public IsPlayerNearBy(float maxDistance, NPCAgentBase context) : base($"Is player within {maxDistance}f?")
        {
            distanceToCheck = maxDistance;
            this.context = context;
        }

        // OnReset() - empty
        protected override void OnReset() {}

        protected override NodeStatus OnRun()
        {
            // Check if the player is nearby
            GameObject player = context.TryGetPlayerWithinRange(distanceToCheck);

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