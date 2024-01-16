using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviorTrees.Internal
{
    public interface IPatroller
    {
        // Reference to agent that inherits from this interface
        public NPCAgentBase agent { get; }
        
        // How long the agent waits at its patrol location until it checks for the player again
        public float patrolStayTime { get; set; }
        
        // The current waypoint, if any
        public int currentWaypointIndex { get; set; }
        
        // List of waypoints to iterate through
        public List<GameObject> waypoints { get; set; }

        /// <summary>
        /// Gets the next waypoint in the waypoints list.
        /// </summary>
        /// <returns>The next waypoint.</returns>
        public GameObject GetNextWayPoint();
        
        /// <summary>
        /// Gets a random waypoint in the waypoints list.
        /// </summary>
        /// <returns>The waypoint.</returns>
        public GameObject GetRandomWayPoint();

        /// <summary>
        /// Initiliazes the waypoints list. Should always be run or else waypoints will not work.
        /// </summary>
        public void InitializeWaypoints();
    }
}