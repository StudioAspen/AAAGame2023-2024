using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;
using EnemyBehaviorTrees.Agents;

namespace EnemyBehaviorTrees.Conditions
{
    // This is a condition that tests whether an NPC agent's current NavigationActivity is a certain NavigationActivity to check for.
    // NOTE: This is a test condition from https://gamedev-resources.com/get-started-with-behavior-trees/ #Condition section.
    // Whether we will actually need this or not will be determined as we build the enemy NPCs
    
    public class IsNavigationActivityTypeOf : Condition
    {
        private NAVIGATION_ACTIVITY activityToCheckFor;

        public IsNavigationActivityTypeOf(NAVIGATION_ACTIVITY activity) :
            base($"Is Navigation Activity {activity}?")
        {
            activityToCheckFor = activity;
        }

        protected override void OnReset() { }

        protected override NODE_STATUS OnRun()
        {
            if (BehaviorTreeTestGameManager.Instance == null || BehaviorTreeTestGameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager and/or NPC is null";
                return NODE_STATUS.Failure;
            }
            
            StatusReason = $"NPC Activity is {activityToCheckFor}";
            return BehaviorTreeTestGameManager.Instance.NPC.MyActivity == activityToCheckFor ? NODE_STATUS.Success : NODE_STATUS.Failure;
        }
    }
}