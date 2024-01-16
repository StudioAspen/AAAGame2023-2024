using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;
using EnemyBehaviorTrees.Internal;

using UnityEngine;

namespace EnemyBehaviorTrees.Nodes
{
    // This is a condition that tests whether an NPC agent's current NavigationActivity is a certain NavigationActivity to check for.
    
    public class IsNavigationActivityTypeOf : Condition
    {
        private NPCAgentBase.NavigationActivity activityToCheckFor;
        // The context is the current NPC agent that is running this node.
        protected NPCAgentBase context { get; }

        public IsNavigationActivityTypeOf(NPCAgentBase.NavigationActivity activity, NPCAgentBase context) :
            base($"Is Navigation Activity, {activity.ToString()}?")
        {
            activityToCheckFor = activity;
            this.context = context;
        }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            StatusReason = $"NPC Activity is {activityToCheckFor}?";

            NPCAgentBase.NavigationActivity NavigationActivityEntry = context.blackboard.GetEntry<NPCAgentBase.NavigationActivity>("Navigation Activity", out bool result);

            if (result == false)
            {
                Debug.LogError($"IsNavigationActivityTypeOf failed for {activityToCheckFor.ToString()}");
                return NodeStatus.FAILURE;
            }
            
            return NavigationActivityEntry == activityToCheckFor ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        }
    }
}