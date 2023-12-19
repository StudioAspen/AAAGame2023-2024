// using EnemyBehaviorTrees.Agents;
// using UnityEngine;
// using WUG.BehaviorTreeVisualizer;
// using EnemyBehaviorTrees.Test;
//
// namespace EnemyBehaviorTrees.Nodes
// {
//     // Condition node to check if any items are nearby based on a set distance.
//     public class AreItemsNearBy : Condition
//     {
//         private float distanceToCheck;
//     
//         // Constructor - just changes the name of the base node object to be descriptive of the distance it checks around the agent
//         public AreItemsNearBy(float maxDistance, NPCAgentBase npc) : base($"Are Items within {maxDistance}f?", npc)
//         {
//             distanceToCheck = maxDistance;
//         }
//     
//         // OnReset() - empty
//         protected override void OnReset() {}
//         
//         protected override NodeStatus OnRun()
//         {
//             // Check npc instance
//             if (Npc == null)
//             {
//                 StatusReason = "NPC is null";
//                 return NodeStatus.FAILURE;
//             }
//             
//             // Get the closest item
//             GameObject item = BehaviorTreeTestGameManager.Instance.GetClosestItem();
//             
//             // Check to see if something is close by
//             if (item == null)
//             {
//                 StatusReason = "No items near by";
//                 return NodeStatus.FAILURE;
//             }
//             // Failure - no items in range
//             else if (Vector3.Distance(item.transform.position, BehaviorTreeTestGameManager.Instance.NPC.transform.position) > distanceToCheck)
//             {
//                 StatusReason = $"No items within range of {distanceToCheck} meters";
//                 return NodeStatus.FAILURE;
//             }
//             
//             // Else, there is something to pick up, return Success
//             return NodeStatus.SUCCESS;
//         }
//     }
// }
