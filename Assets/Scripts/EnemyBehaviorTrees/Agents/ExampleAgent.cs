using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyBehaviorTrees.Internal;
using EnemyBehaviorTrees.Nodes;
using Sequence = EnemyBehaviorTrees.Nodes.Sequence;

namespace EnemyBehaviorTrees.Agents
{
    /// <summary>
    /// This ExampleAgent class inherits from the NPCAgentBase class, giving it its own blackboard.
    ///
    /// <p>Every single NPC agent must do at least two things, inherit from abstract class NPCAgentBase, and implement the method GenerateBehaviorTree()
    /// in order to function properly.</p>
    /// <p>This class specifically has its own behavior tree and its nodes, as well as the functionality to patrol, aggro, deaggro, and hit the player.</p>
    /// <p>It also has a list of waypoints to patrol to.</p>
    /// </summary>
    public class ExampleAgent : NPCAgentBase, IPatroller, IHostile
    {
        // Needed to appease interfaces
        public NPCAgentBase agent { get; set; }
        
        [Tooltip("How long the agent waits at its patrol location until it checks for the player again")]
        public float patrolStayTime { get; set; } = 2f;
        
        [Tooltip("How far away the agent checks from itself for the player")]
        public float playerAggroRange { get; set; } = 5f;
        
        [Tooltip("How far away the agent checks from itself to see if it deaggros because the player ran far enough away")]
        public float playerDeAggroRange { get; set; } = 7f;
        
        [Tooltip("How long the agent idles for whenever it player goes into deaggro range")]
        public float idleTimeSecs { get; set; } = 2f;

        [Tooltip("How far away the agent needs to be to start hitting the player")]
        public float playerHitRange { get; set; } = 0.5f;

        public List<GameObject> waypoints { get; set; } = new List<GameObject>();
        public int currentWaypointIndex { get; set; } = 0;
        
        /// <summary>
        /// Every NPC Agent needs its own GenerateBehaviorTree function, which inside must contain a behavior tree object consisting of a tree of nodes.
        /// </summary>
        public override void GenerateBehaviorTree()
        {
            InitializeWaypoints();
            agent = this;
            
            BehaviorTree = new Selector("Control NPC",
                                // Navigation branch: Idle
                                new Sequence("Idle",
                                    // NOTE: FOLLOW ENUM CASING CONVENTION FOR ANY STRING VALUE THAT'S GOING TO BE USED A LOT FOR CLARITY'S SAKE.
                                    // This prevents any unseeable bugs when multiple people work with them.
                                    new IsNavigationActivityTypeOf(NavigationActivity.IDLE, this),
                                    new NodeTimer(idleTimeSecs, 
                                        new SetNavigationActivityTo(NavigationActivity.PATROL, this))),
                                // Navigation branch: Look for player
                                new Sequence("Look for player",
                                    new IsNavigationActivityTypeOf(NavigationActivity.LOOK_FOR_PLAYER, this),
                                    new Selector("Is player in aggro range?",
                                        // AGENT AGGRO
                                        new Sequence("Look for player", 
                                            new IsPlayerNearBy(playerAggroRange, this),
                                            new SetNavigationActivityTo(NavigationActivity.CHASE, this)),
                                        new SetNavigationActivityTo(NavigationActivity.PATROL, this))),
                                // Navigation branch: Chase
                                new Sequence("Chase Player",
                                    new IsNavigationActivityTypeOf(NavigationActivity.CHASE, this),
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
                                                new SetNavigationActivityTo(NavigationActivity.IDLE, this)),
                                            new SetNavigationActivityTo(NavigationActivity.LOOK_FOR_PLAYER, this)))),
                                // Navigation branch: Patrol
                                new Sequence("Move to Waypoint",
                                    new IsNavigationActivityTypeOf(NavigationActivity.PATROL, this),
                                    new NavigateToRandomWaypoint(this),
                                    new NodeTimer(patrolStayTime,
                                        new SetNavigationActivityTo(NavigationActivity.LOOK_FOR_PLAYER, this))));
        }

        #region IPATROLLER METHODS
        
        public GameObject GetNextWayPoint()
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                int newIndex = currentWaypointIndex++;

                if (newIndex == waypoints.Count) { newIndex = 0; }
                
                return waypoints[currentWaypointIndex++];
            }
            
            return null;
        }

        public GameObject GetRandomWayPoint()
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                return waypoints[Random.Range(0, waypoints.Count)];
            }

            return null;
        }

        public void InitializeWaypoints()
        {
            Transform waypointsChild = transform.Find("Waypoints");

            for (int i = 0; i < waypointsChild.childCount; i++)
            {
                waypoints.Add(waypointsChild.GetChild(i).gameObject);
            }

            if (waypoints.Count <= 0)
            {
                Debug.Log("Please have an empty child game object named Waypoints that contains the transforms of the waypoints this agent should go to.");
            }
        }

        #endregion

        #region IHOSTILE METHODS

        public GameObject TryGetPlayerWithinRange(float range)
        {
            Collider[] objs = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Player"));
            
            if (objs.Length > 0 && objs[0] != null)
            {
                return objs[0].gameObject;
            }

            return null;
        }

        #endregion
    }
}
