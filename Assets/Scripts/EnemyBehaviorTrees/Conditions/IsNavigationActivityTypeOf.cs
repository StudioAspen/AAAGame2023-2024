using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;
using EnemyBehaviorTrees.Managers;

namespace EnemyBehaviorTrees.Nodes
{
    // This is a condition that tests whether an NPC agent's current NavigationActivity is a certain NavigationActivity to check for.
    // NOTE: This is a test condition from https://gamedev-resources.com/get-started-with-behavior-trees/ #Condition section.
    
    public class IsNavigationActivityTypeOf : Condition
    {
        private NavigationActivity activityToCheckFor;

        public IsNavigationActivityTypeOf(NavigationActivity activity) :
            base($"Is Navigation Activity {activity}?")
        {
            activityToCheckFor = activity;
        }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            if (EnemyBehaviorTreeGameManager.Instance == null || EnemyBehaviorTreeGameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager and/or NPC is null";
                return NodeStatus.FAILURE;
            }
            
            StatusReason = $"NPC Activity is {activityToCheckFor}";
            return EnemyBehaviorTreeGameManager.Instance.NPC.MyActivity == activityToCheckFor ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        }
    }
}