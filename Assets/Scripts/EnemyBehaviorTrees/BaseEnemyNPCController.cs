using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Nodes;
using EnemyBehaviorTrees.Managers;
using UnityEngine.UI;

namespace EnemyBehaviorTrees.Agents
{
    public enum NavigationActivity
    {
        IDLE,
        LOOK_FOR_PLAYER,
        CHASE,
        PATROL
    }

    public class BaseEnemyNPCController : MonoBehaviour, IBehaviorTree
    {
        public NavMeshAgent MyNavMesh { get; private set; }
        public NavigationActivity MyActivity { get; set; }
        public NodeBase BehaviorTree { get; set; }

        [Header("Agent Modification")] 
        [Tooltip("How long the agent waits at its patrol location until it checks for the player again")]
        public float patrolStayTime = 2f;
        
        [Tooltip("How far away the agent checks from itself for the player")]
        public float playerAggroRange = 5f;
        
        [Tooltip("How far away the agent checks from itself to see if it deaggros because the player ran far enough away")]
        public float playerDeAggroRange = 7f;
        
        [Tooltip("How long the agent idles for whenever it player goes into deaggro range")]
        public float idleTimeSecs = 2f;

        [Tooltip("How far away the agent needs to be to start hitting the player")]
        public float playerHitRange = 0.5f;

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
                                // NAVIGATION_ACTIVITY.IDLE
                                new Sequence("Idle",
                                    new IsNavigationActivityTypeOf(NavigationActivity.IDLE),
                                    new Timer(idleTimeSecs, 
                                        new SetNavigationActivityTo(NavigationActivity.PATROL))),
                                // NAVIGATION_ACTIVITY.LOOK_FOR_PLAYER
                                new Sequence("Look for player",
                                    new IsNavigationActivityTypeOf(NavigationActivity.LOOK_FOR_PLAYER),
                                    new Selector("Is player in aggro range?",
                                        // AGENT AGGRO
                                        new Sequence("Look for player", 
                                            new IsPlayerNearBy(playerAggroRange),
                                            new SetNavigationActivityTo(NavigationActivity.CHASE)),
                                        new SetNavigationActivityTo(NavigationActivity.PATROL))),
                                // NAVIGATION_ACTIVITY.CHASE
                                new Sequence("Chase Player",
                                    new IsNavigationActivityTypeOf(NavigationActivity.CHASE),
                                    new Selector("Is player still in aggro range?",
                                        new Sequence("Check if player still in aggro range",
                                            new IsPlayerNearBy(playerAggroRange),
                                            new Selector("Is player within hit range? Attack/Chase",
                                                new Sequence("Check if player is in attack range",
                                                    new IsPlayerNearBy(playerHitRange),
                                                    new AttackPlayer()),
                                                new NavigateToDestination())),
                                        new Selector("Is player within deaggro range?",
                                            new Sequence("Check if player in deaggro range",
                                                new IsPlayerNearBy(playerDeAggroRange),
                                                new SetNavigationActivityTo(NavigationActivity.IDLE)),
                                            new SetNavigationActivityTo(NavigationActivity.LOOK_FOR_PLAYER)))),
                                // NAVIGATION_ACTIVITY.PATROL
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