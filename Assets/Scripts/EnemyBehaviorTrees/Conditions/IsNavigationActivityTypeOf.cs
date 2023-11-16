using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;
using UnityEngine;
using NavigationActivity = EnemyBehaviorTrees.Agents.NPCAgentBase.NavigationActivity;

namespace EnemyBehaviorTrees.Nodes
{
    // This is a condition that tests whether an NPC agent's current NavigationActivity is a certain NavigationActivity to check for.
    
    public class IsNavigationActivityTypeOf : Condition
    {
        private NavigationActivity activityToCheckFor;
        private NPCAgentBlackboard blackboard;
        private NPCAgentBase npc;

        public IsNavigationActivityTypeOf(NavigationActivity activity, int npcIndex) :
            base($"Is Navigation Activity, {activity}?", npcIndex)
        {
            activityToCheckFor = activity;
            
            blackboard = GameObject.Find("NPC Blackboard").GetComponent<NPCAgentBlackboard>();
            if (blackboard == null) { Debug.LogError("Please create a blackboard Game Object in the hierarchy called 'NPC Blackboard' with an 'NPCAgentBlackboard' component on it."); }
            
            npc = blackboard.NPCs[npcIndex];
        }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            StatusReason = $"NPC Activity is {activityToCheckFor}";
            return npc.MyActivity == activityToCheckFor ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        }
    }
}