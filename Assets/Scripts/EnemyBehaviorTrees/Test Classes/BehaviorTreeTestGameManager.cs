using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;

namespace EnemyBehaviorTrees.Test
{
    public class BehaviorTreeTestGameManager : MonoBehaviour
    {
        public static BehaviorTreeTestGameManager Instance;

        public BaseEnemyNPCController NPC { get; private set; }
        private List<GameObject> waypoints = new List<GameObject>();
        private List<GameObject> items = new List<GameObject>();

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
            items = GameObject.FindGameObjectsWithTag("Item").ToList();

            waypoints = waypoints.Shuffle();

            NPC = FindObjectOfType<BaseEnemyNPCController>();
        }

        /// <summary>
        /// Sorts the remaining items by distance to NPC and pops the next one off the list
        /// </summary>
        /// <returns>Closest item</returns>
        public GameObject GetClosestItem()
        {
            return items.OrderBy(x => Vector3.Distance(x.transform.position, NPC.transform.position)).FirstOrDefault();
        }

        /// <summary>
        /// Removes an item from the list and scene
        /// </summary>
        /// <param name="item"></param>
        public void PickupItem(GameObject item)
        {
            items.Remove(item);

            Destroy(item);
        }

        /// <summary>
        /// Finds the next waypoint on the list. This is 'random' due to shuffling on Start.
        /// </summary>
        /// <returns>Next waypoint</returns>
        public GameObject GetNextWayPoint()
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                GameObject nextWayPoint = waypoints[0];
                waypoints.RemoveAt(0);

                return nextWayPoint;
            }

            return null;
        }

    }
}