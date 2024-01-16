using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Internal;
using UnityEngine;

namespace EnemyBehaviorTrees.Nodes
{
    // This action changes an NPC Agent's current Navigation Activity to a new one or reaffirms it.
    
    public class SetNavigationActivityTo : Node
    {
        private NPCAgentBase.NavigationActivity newActivity;
        // The context is the current NPC agent that is running this node.
        protected NPCAgentBase context { get; }
    
        // Constructor - declare which activity to change to
        public SetNavigationActivityTo(NPCAgentBase.NavigationActivity newActivity, NPCAgentBase context)
        {
            this.newActivity = newActivity;
            Name = $"Set NavigationActivity to {newActivity.ToString()}";
            this.context = context;
        }
        
        // OnReset() - empty
        protected override void OnReset() { }
        
        protected override NodeStatus OnRun()
        {
            // Set Agent's Navigation Activity to new activity and return Success
            context.blackboard.SetEntry("Navigation Activity", newActivity);
    
            Debug.Log($"{context.name}'s current navigation activity is {newActivity.ToString()}");
            return NodeStatus.SUCCESS;
        }
    }
}
