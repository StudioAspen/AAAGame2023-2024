using EnemyBehaviorTrees.Agents;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;

namespace EnemyBehaviorTrees.Conditions
{
    // Condition node to check if any items are nearby based on a set distance.
    // 
    
    
    public class AreItemsNearBy : Condition
    {
        private float distanceToCheck;
    
        // Constructor - just changes the name of the base node object to be descriptive of the distance it checks around the agent
        public AreItemsNearBy(float maxDistance) : base($"Are Items within {maxDistance}f?")
        {
            distanceToCheck = maxDistance;
        }
    
        // OnReset() - empty
        protected override void OnReset() { }
        
        protected override NODE_STATUS OnRun()
        {
            // Check for references
            if (BehaviorTreeTestGameManager.Instance == null || BehaviorTreeTestGameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager and/or NPC is null";
                return NODE_STATUS.Failure;
            }
            
            // Get the closest item
            GameObject item = BehaviorTreeTestGameManager.Instance.GetClosestItem();
            
            // Check to see if something is close by
            if (item == null)
            {
                StatusReason = "No items near by";
                return NODE_STATUS.Failure;
            }
            // Failure - no items in range
            else if (Vector3.Distance(item.transform.position, BehaviorTreeTestGameManager.Instance.NPC.transform.position) > distanceToCheck)
            {
                StatusReason = $"No items within range of {distanceToCheck} meters";
                return NODE_STATUS.Failure;
            }
            
            // Else, there is something to pick up, return Success
            return NODE_STATUS.Success;
        }
    }
}
