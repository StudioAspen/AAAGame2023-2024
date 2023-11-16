using UnityEngine;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;

namespace EnemyBehaviorTrees.Nodes
{
    // This is a condition that tests whether an NPC agent's current NavigationActivity is a certain NavigationActivity to check for.
    // NOTE: This is a test condition from https://gamedev-resources.com/get-started-with-behavior-trees/ #Condition section.

    public class IsPlayerNearBy : Condition
    {
        private float distanceToCheck;
        private NPCAgentBlackboard blackboard;

        // Constructor - just changes the name of the base node object to be descriptive of the distance it checks around the agent
        public IsPlayerNearBy(float maxDistance, int index) : base($"Is player within {maxDistance}f?", index)
        {
            distanceToCheck = maxDistance;
            
            blackboard = GameObject.Find("NPC Blackboard").GetComponent<NPCAgentBlackboard>();
            if (blackboard == null) { Debug.Log("Please create a blackboard Game Object called 'NPC Blackboard' with an 'NPCAgentBlackboard' component on it."); }
        }

        // OnReset() - empty
        protected override void OnReset() {}

        protected override NodeStatus OnRun()
        {
            // Check if the player is nearby
            GameObject player = blackboard.GetPlayerWithinRange(npcIndex);

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