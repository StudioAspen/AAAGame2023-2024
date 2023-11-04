using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;
using Random = UnityEngine.Random;

namespace EnemyBehaviorTrees.Managers
{
    public class EnemyBehaviorTreeGameManager : MonoBehaviour
    {
        public static EnemyBehaviorTreeGameManager Instance;

        public BaseEnemyNPCController NPC { get; private set; }
        private List<GameObject> waypoints = new List<GameObject>();
        // private List<GameObject> items = new List<GameObject>();
        private GameObject player;

        private void Awake()
        {
            // Singleton pattern. Ensures only one version of this lives in the scene
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint").ToList();
            // items = GameObject.FindGameObjectsWithTag("Item").ToList();
            player = GameObject.FindGameObjectWithTag("Player");
            
            waypoints = waypoints.Shuffle();

            NPC = FindObjectOfType<BaseEnemyNPCController>();
        }

        
        /// <summary>
        /// Check if player is within a certain range using Physics.CheckSphere
        /// </summary>
        /// <returns>If the player is within a range</returns>
        public GameObject GetPlayerWithinRange(float maxDistance)
        {
            LayerMask playerMask = LayerMask.GetMask("Player");
            return Physics.CheckSphere(NPC.transform.position, maxDistance, playerMask, QueryTriggerInteraction.Ignore) ? player : null;
        }


        /// <summary>
        /// Finds the next waypoint on the list. This is 'random' due to shuffling at the start and also randomly chosen on decision.
        /// </summary>
        /// <returns>Next waypoint</returns>
        public GameObject GetNextWayPoint()
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                int randomWaypointIndex = Random.Range(0, waypoints.Count);
                GameObject nextWayPoint = waypoints[randomWaypointIndex];

                return nextWayPoint;
            }

            return null;
        }

        /// CURRENTLY UNUSED FUNCTIONS FROM TEST:
        
        // /// <summary>
        // /// Sorts the remaining items by distance to NPC and pops the next one off the list
        // /// </summary>
        // /// <returns>Closest item</returns>
        // public bool GetClosestItem()
        // {
        //     return items.OrderBy(x => Vector3.Distance(x.transform.position, NPC.transform.position)).FirstOrDefault();
        // }
        
        // /// <summary>
        // /// Removes an item from the list and scene
        // /// </summary>
        // /// <param name="item"></param>
        // public void PickupItem(GameObject item)
        // {
        //     items.Remove(item);
        //
        //     Destroy(item);
        // }
    }
}
