using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;
using EnemyBehaviorTrees.Internal;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

namespace EnemyBehaviorTrees.Nodes
{
    // This is a condition that tests whether an NPC agent's current NavigationActivity is a certain NavigationActivity to check for.
    
    public class IsNavigationActivityTypeOf : Condition
    {
        private string activityToCheckFor;
        // The context is the current NPC agent that is running this node.
        protected NPCAgentBase context { get; }

        public IsNavigationActivityTypeOf(string activity, NPCAgentBase context) :
            base($"Is Navigation Activity, {activity}?")
        {
            activityToCheckFor = activity;
            this.context = context;
        }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            StatusReason = $"NPC Activity is {activityToCheckFor}?";

            bool NavigationActivityEntry = context.blackboard.GetEntry<string>("Navigation Activity", out string value);

            if (value == default)
            {
                Debug.LogError($"IsNavigationActivityTypeOf failed for {activityToCheckFor}");
                return NodeStatus.FAILURE;
            }
            
            return value == activityToCheckFor ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        }
    }
}