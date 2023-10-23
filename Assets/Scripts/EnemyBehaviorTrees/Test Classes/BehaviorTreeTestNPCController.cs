// using System.Collections;
// using UnityEngine;
// using UnityEngine.AI;
// using WUG.BehaviorTreeVisualizer;
// using EnemyBehaviorTrees.Nodes;
//
// namespace EnemyBehaviorTrees.Test
// {
//     public enum NavigationActivityTest
//     {
//         WAYPOINT, 
//         PICKUP_ITEM
//     }
//
//     public class BehaviorTreeTestNPCController : MonoBehaviour, IBehaviorTree
//     {
//         public NavMeshAgent MyNavMesh { get; private set; }
//         public NavigationActivityTest MyActivity { get; set; }
//         public NodeBase BehaviorTree { get; set; }
//
//         private Coroutine behaviorTreeRoutine;
//         private YieldInstruction waitTime = new WaitForSeconds(.1f);
//
//         private void Start()
//         {
//             MyNavMesh = GetComponent<NavMeshAgent>();
//             MyActivity = NavigationActivityTest.WAYPOINT;
//
//             GenerateBehaviorTree();
//
//             if (behaviorTreeRoutine == null && BehaviorTree != null)
//             {
//                 behaviorTreeRoutine = StartCoroutine(RunBehaviorTree());
//             }
//         }
//
//         private void GenerateBehaviorTree()
//         {
//             BehaviorTree = new Selector("Control NPC",
//                                 new Sequence("Pickup Item",
//                                     new IsNavigationActivityTypeOf(NavigationActivityTest.PICKUP_ITEM),
//                                     new Selector("Look for or move to items",
//                                         new Sequence("Look for items",
//                                             new Inverter("Inverter",
//                                                 new AreItemsNearBy(5f)),
//                                             new SetNavigationActivityTo(NavigationActivity.WAYPOINT)),
//                                         new Sequence("Navigate to Item",
//                                             new NavigateToDestination()))),
//                                 new Sequence("Move to Waypoint",
//                                     new IsNavigationActivityTypeOf(NavigationActivity.WAYPOINT),
//                                     new NavigateToDestination(),
//                                     new Timer(2f,
//                                         new SetNavigationActivityTo(NavigationActivity.PICKUP_ITEM))));
//         }
//
//         private IEnumerator RunBehaviorTree()
//         {
//             while (enabled)
//             {
//                 if (BehaviorTree == null)
//                 {
//                     $"{this.GetType().Name} is missing Behavior Tree. Did you set the BehaviorTree property?".BTDebugLog();
//                     continue;
//                 }
//
//                 (BehaviorTree as Node).Run();
//
//                 yield return waitTime;
//             }
//         }
//
//         private void OnDestroy()
//         {
//             if (behaviorTreeRoutine != null)
//             {
//                 StopCoroutine(behaviorTreeRoutine);
//             }
//         }
//
//     }
// }