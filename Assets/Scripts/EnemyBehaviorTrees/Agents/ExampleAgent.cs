using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyBehaviorTrees.Internal;
using EnemyBehaviorTrees.Nodes;

namespace EnemyBehaviorTrees.Agents
{
    /// <summary>
    /// This ExampleAgent class inherits from the NPCAgentBase class, giving it its own blackboard.
    /// <p>This class specifically has its own behavior tree and its nodes, as well as the functionality to patrol, aggro, deaggro, and hit the player.</p>
    /// <p>It also has a list of waypoints to patrol to.</p>
    /// </summary>
    public class ExampleAgent : NPCAgentBase, IPatroller, IHostile
    {
        // Reference to agent that inherits from this interface
        public NPCAgentBase agent { get; set; }
        
        [Tooltip("How long the agent waits at its patrol location until it checks for the player again")]
        public float patrolStayTime { get; } = 2f;
        
        [Tooltip("How far away the agent checks from itself for the player")]
        public float playerAggroRange { get; } = 5f;
        
        [Tooltip("How far away the agent checks from itself to see if it deaggros because the player ran far enough away")]
        public float playerDeAggroRange { get; } = 7f;
        
        [Tooltip("How long the agent idles for whenever it player goes into deaggro range")]
        public float idleTimeSecs { get; } = 2f;

        [Tooltip("How far away the agent needs to be to start hitting the player")]
        public float playerHitRange { get; } = 0.5f;

        public List<GameObject> waypoints { get; set; } = new List<GameObject>();
        public int currentWaypointIndex { get; set; } = 0;

        public override void GenerateBehaviorTree()
        {
            BehaviorTree = new Selector("Control NPC",
                                // Navigation branch: Idle
                                new Sequence("Idle",
                                    new IsNavigationActivityTypeOf("Idle", this),
                                    new Timer(idleTimeSecs, 
                                        new SetNavigationActivityTo("Patrol", this))),
                                // Navigation branch: Look for player
                                new Sequence("Look for player",
                                    new IsNavigationActivityTypeOf("Look for player", this),
                                    new Selector("Is player in aggro range?",
                                        // AGENT AGGRO
                                        new Sequence("Look for player", 
                                            new IsPlayerNearBy(playerAggroRange, this),
                                            new SetNavigationActivityTo("Chase", this)),
                                        new SetNavigationActivityTo("Patrol", this))),
                                // Navigation branch: Chase
                                new Sequence("Chase Player",
                                    new IsNavigationActivityTypeOf("Chase", this),
                                    new Selector("Is player still in aggro range?",
                                        new Sequence("Check if player still in aggro range",
                                            new IsPlayerNearBy(playerAggroRange, this),
                                            new Selector("Is player within hit range? Attack/Chase",
                                                new Sequence("Check if player is in attack range",
                                                    new IsPlayerNearBy(playerHitRange, this),
                                                    new AttackPlayer()),
                                                new NavigateToRandomWaypoint(this))),
                                        new Selector("Is player within deaggro range?",
                                            new Sequence("Check if player in deaggro range",
                                                new IsPlayerNearBy(playerDeAggroRange, this),
                                                new SetNavigationActivityTo("Idle", this)),
                                            new SetNavigationActivityTo("Look for player", this)))),
                                // Navigation branch: Patrol
                                new Sequence("Move to Waypoint",
                                    new IsNavigationActivityTypeOf("Patrol", this),
                                    new NavigateToRandomWaypoint(this),
                                    new Timer(patrolStayTime,
                                        new SetNavigationActivityTo("Look for player", this))));
        }

        #region IPATROLLER METHODS
        
        public override GameObject GetNextWayPoint()
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                int newIndex = currentWaypointIndex++;

                if (newIndex == waypoints.Count) { newIndex = 0; }
                
                return waypoints[currentWaypointIndex++];
            }
            
            return null;
        }

        public override GameObject GetRandomWayPoint()
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                return waypoints[Random.Range(0, waypoints.Count)];
            }

            return null;
        }

        #endregion

        #region IHOSTILE METHODS

        public override GameObject TryGetPlayerWithinRange(float range)
        {
            Collider[] objs = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Player"));
            return objs[0] != null ? objs[0].gameObject : null;
        }

        #endregion
    }
}
