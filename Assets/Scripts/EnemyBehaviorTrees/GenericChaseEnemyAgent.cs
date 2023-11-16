using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Nodes;
using UnityEngine.UI;
using NavigationActivity = EnemyBehaviorTrees.Agents.NPCAgentBase.NavigationActivity;

namespace EnemyBehaviorTrees.Agents
{
    public class GenericChaseEnemyAgent : NPCAgentBase, IBehaviorTree
    {
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

        public override void GenerateBehaviorTree()
        {
            BehaviorTree = new Selector("Control NPC",
                                // NAVIGATION_ACTIVITY.IDLE
                                new Sequence("Idle",
                                    new IsNavigationActivityTypeOf(NavigationActivity.IDLE, index),
                                    new Timer(idleTimeSecs, 
                                        new SetNavigationActivityTo(NavigationActivity.PATROL, index))),
                                // NAVIGATION_ACTIVITY.LOOK_FOR_PLAYER
                                new Sequence("Look for player",
                                    new IsNavigationActivityTypeOf(NavigationActivity.LOOK_FOR_PLAYER, index),
                                    new Selector("Is player in aggro range?",
                                        // AGENT AGGRO
                                        new Sequence("Look for player", 
                                            new IsPlayerNearBy(playerAggroRange, index),
                                            new SetNavigationActivityTo(NavigationActivity.CHASE, index)),
                                        new SetNavigationActivityTo(NavigationActivity.PATROL, index))),
                                // NAVIGATION_ACTIVITY.CHASE
                                new Sequence("Chase Player",
                                    new IsNavigationActivityTypeOf(NavigationActivity.CHASE, index),
                                    new Selector("Is player still in aggro range?",
                                        new Sequence("Check if player still in aggro range",
                                            new IsPlayerNearBy(playerAggroRange, index),
                                            new Selector("Is player within hit range? Attack/Chase",
                                                new Sequence("Check if player is in attack range",
                                                    new IsPlayerNearBy(playerHitRange, index),
                                                    new AttackPlayer()),
                                                new NavigateToDestination(index))),
                                        new Selector("Is player within deaggro range?",
                                            new Sequence("Check if player in deaggro range",
                                                new IsPlayerNearBy(playerDeAggroRange, index),
                                                new SetNavigationActivityTo(NavigationActivity.IDLE, index)),
                                            new SetNavigationActivityTo(NavigationActivity.LOOK_FOR_PLAYER, index)))),
                                // NAVIGATION_ACTIVITY.PATROL
                                new Sequence("Move to Waypoint",
                                    new IsNavigationActivityTypeOf(NavigationActivity.PATROL, index),
                                    new NavigateToDestination(index),
                                    new Timer(patrolStayTime,
                                        new SetNavigationActivityTo(NavigationActivity.LOOK_FOR_PLAYER, index))));
        }
    }
}