using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Nodes;
using EnemyBehaviorTrees.Managers;

namespace EnemyBehaviorTrees.Agents
{
    public enum NavigationActivity
    {
        LOOK_FOR_PLAYER,
        PATROL
    }

    public class BaseEnemyNPCController : MonoBehaviour, IBehaviorTree
    {
        public NavMeshAgent MyNavMesh { get; private set; }
        public NavigationActivity MyActivity { get; set; }
        public NodeBase BehaviorTree { get; set; }

        [Header("Agent Modification")] 
        [Tooltip("How far away the agent checks from itself for the player")]
        public float playerCheckRange = 5f;
        [Tooltip("How long the agent waits at its patrol location until it checks for the player again")]
        public float patrolStayTime = 2f;

        private Coroutine behaviorTreeRoutine;
        private YieldInstruction waitTime = new WaitForSeconds(.1f);

        private void Start()
        {
            MyNavMesh = GetComponent<NavMeshAgent>();
            MyActivity = NavigationActivity.LOOK_FOR_PLAYER;

            GenerateBehaviorTree();

            if (behaviorTreeRoutine == null && BehaviorTree != null)
            {
                behaviorTreeRoutine = StartCoroutine(RunBehaviorTree());
            }
        }

        private void GenerateBehaviorTree()
        {
            BehaviorTree = new Selector("Control NPC",
                                new Sequence("Look for player",
                                    new IsNavigationActivityTypeOf(NavigationActivity.LOOK_FOR_PLAYER),
                                    new Selector("Look for or move to player",
                                        new Sequence("Look for player",
                                            new Inverter("Inverter", 
                                                new IsPlayerNearBy(playerCheckRange)),
                                            // Player is not nearby, patrol to next point
                                            new SetNavigationActivityTo(NavigationActivity.PATROL)),
                                        new Sequence("Navigate to player",
                                            new NavigateToDestination()))),
                                new Sequence("Move to Waypoint",
                                    new IsNavigationActivityTypeOf(NavigationActivity.PATROL),
                                    new NavigateToDestination(),
                                    new Timer(patrolStayTime,
                                        new SetNavigationActivityTo(NavigationActivity.LOOK_FOR_PLAYER))));
        }

        private IEnumerator RunBehaviorTree()
        {
            while (enabled)
            {
                if (BehaviorTree == null)
                {
                    $"{this.GetType().Name} is missing Behavior Tree. Did you set the BehaviorTree property?".BTDebugLog();
                    continue;
                }

                (BehaviorTree as Node).Run();

                yield return waitTime;
            }
        }

        private void OnDestroy()
        {
            if (behaviorTreeRoutine != null)
            {
                StopCoroutine(behaviorTreeRoutine);
            }
        }

    }
}