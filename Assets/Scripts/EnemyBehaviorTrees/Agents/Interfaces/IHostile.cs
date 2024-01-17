using UnityEngine;

namespace EnemyBehaviorTrees.Internal
{
    public interface IHostile
    {
        // Reference to agent that inherits from this interface
        public NPCAgentBase agent { get; }
        
        [Tooltip("How far away the agent checks from itself for the player")]
        public float playerAggroRange { get; set; }
        
        [Tooltip("How far away the agent checks from itself to see if it deaggros because the player ran far enough away")]
        public float playerDeAggroRange { get; set; }
        
        [Tooltip("How long the agent idles for whenever it player goes into deaggro range")]
        public float idleTimeSecs { get; set; }

        [Tooltip("How far away the agent needs to be to start hitting the player")]
        public float playerHitRange { get; set; }
        
        /// <summary>
        /// Checks if the player is within a given range. If it is, then we return the player GameObject.
        /// </summary>
        /// <returns> The Player GameObject if it's within range. If not, it returns null. </returns>
        public GameObject TryGetPlayerWithinRange(float range);
    }
}