using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;
using EnemyBehaviorTrees.Internal;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

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
        }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            StatusReason = $"NPC Activity is {activityToCheckFor}";
            return this.context.blackboard.GetEntry("Navigation Activity") == activityToCheckFor ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        }
    }
}