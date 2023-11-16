using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;
using EnemyBehaviorTrees.Agents;
using UnityEngine;
using NavigationActivity = EnemyBehaviorTrees.Agents.NPCAgentBase.NavigationActivity;

namespace EnemyBehaviorTrees.Nodes
{
    // This action changes an NPC Agent's current Navigation Activity to a new one or reaffirms it.
    
    public class SetNavigationActivityTo : Node
    {
        private NavigationActivity newActivity;
        private NPCAgentBlackboard blackboard;
        private NPCAgentBase npc;
    
        // Constructor - declare which activity to change to
        public SetNavigationActivityTo(NavigationActivity newActivity, int npcIndex)
        {
            this.newActivity = newActivity;
            Name = $"Set NavigationActivity to {newActivity}";
            
            blackboard = GameObject.Find("NPC Blackboard").GetComponent<NPCAgentBlackboard>();
            if (blackboard == null) { Debug.Log("Please create a blackboard Game Object called 'NPC Blackboard' with an 'NPCAgentBlackboard' component on it."); }
            
            npc = blackboard.NPCs[npcIndex];
        }
        
        // OnReset() - empty
        protected override void OnReset() { }
        
        protected override NodeStatus OnRun()
        {
            // Set Agent's Navigation Activity to new activity and return Success
            npc.MyActivity = newActivity;
    
            Debug.Log($"Enemy's current navigation activity is {newActivity}");
            return NodeStatus.SUCCESS;
        }
    }
}
