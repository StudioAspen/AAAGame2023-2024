using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Agents;
using Random = UnityEngine.Random;

namespace EnemyBehaviorTrees.Agents
{
    public class NPCAgentBlackboard : MonoBehaviour
    {
        // Having a npc array allows us to not have to pass in a reference to the npc in each leaf node in a behavior tree. Saves a bit of memory overhead.
        public List<NPCAgentBase> NPCs { get; private set; }
        private List<GameObject> waypoints = new List<GameObject>();
        // private List<GameObject> items = new List<GameObject>();
        private GameObject player;
        LayerMask playerMask;

        void Awake()
        {
            GameObject[] NPCObjects = GameObject.FindGameObjectsWithTag("NPC").ToList();
        }
        
        void Start()
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint").ToList();
            // items = GameObject.FindGameObjectsWithTag("Item").ToList();
            player = GameObject.FindGameObjectWithTag("Player");
            
            waypoints = waypoints.Shuffle();
            
            playerMask = LayerMask.GetMask("Player");
            
            // assign indicies to npcs in scene
            // for (int i = 0; i < NPCs.Length; i++)
            // {
            //     NPCs[i].index = i;
            // }
        }

        // When we spawn a new agent, we want to add it to the array of NPCs
        public void NPCSpawned(NPCAgentBase agent)
        {
            NPCs.Append(agent);
        }

        
        /// <summary>
        /// Check if player is within a certain range using Physics.CheckSphere
        /// </summary>
        /// <returns>If the player is within a range</returns>
        public GameObject GetPlayerWithinRange(int npcIndex)
        {
            // npc index out of range
            if (npcIndex > NPCs.Length) { return null; }
            
            NPCAgentBase npc = NPCs[npcIndex];
            return Physics.CheckSphere(npc.transform.position, npc.playerCheckDistance, playerMask, QueryTriggerInteraction.Ignore) ? player : null;
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
