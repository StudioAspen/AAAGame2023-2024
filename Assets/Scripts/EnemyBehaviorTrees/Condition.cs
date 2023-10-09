using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;

namespace EnemyBehaviorTrees
{
    // Base superclass for Condition nodes. Condition nodes are nodes that test a condition in the game in order to choose which child to execute.
    // e.g. how far is the player from this agent? if x units away, chase, if y units away, attack, things like that.
    
    // This code just allows the Behavior Tree Visualizer to stylize all conditions a specific way
    public abstract class Condition : Node
    {
        public Condition(string name)
        {
            Name = name;
        }
    }
    
    
    // NOTE: These are test conditions from https://gamedev-resources.com/get-started-with-behavior-trees/ #Condition section.
    
    /*
    public class IsNavigationActivityTypeOf : Condition
    {
        private NavigationActivity m_ActivityToCheckFor;

        public IsNavigationActivityTypeOf(NavigationActivity activity) :
            base($"Is Navigation Activity {activity}?")
        {
            m_ActivityToCheckFor = activity;
        }
        
        protected override void OnReset() { }
        
        protected override NodeStatus OnRun()
        {
            if (GameManager.Instance == null || GameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager and/or NPC is null";
                return NodeStatus.Failure;
            }
            StatusReason = $"NPC Activity is {m_ActivityToCheckFor}";
            return GameManager.Instance.NPC.MyActivity == m_ActivityToCheckFor ? NodeStatus.Success : NodeStatus.Failure; 
        }
    }
    */
    
    /*
     public class AreItemsNearBy : Condition
    {
        private float m_DistanceToCheck;
        
        public AreItemsNearBy(float maxDistance) : base($"Are Items within {maxDistance}f?") 
        { 
            m_DistanceToCheck = maxDistance; 
        }
        
        protected override void OnReset() { }
        protected override NodeStatus OnRun()
        {
            //Check for references
            if (GameManager.Instance == null || GameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager and/or NPC is null";
                return NodeStatus.Failure;
            }
            //Get the closest item
            GameObject item = GameManager.Instance.GetClosestItem();
            //Check to see if something is close by
            if (item == null)
            {
                StatusReason = "No items near by";
                return NodeStatus.Failure;
            }
            else if (Vector3.Distance(item.transform.position, 
                GameManager.Instance.NPC.transform.position) > m_DistanceToCheck)
            {
                StatusReason = $"No items within range of {m_DistanceToCheck} meters";
                return NodeStatus.Failure;
            }
            return NodeStatus.Success;
        }
    }
    */
}
