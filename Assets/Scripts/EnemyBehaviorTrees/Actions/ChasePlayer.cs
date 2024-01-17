using EnemyBehaviorTrees.Internal;
using EnemyBehaviorTrees.Agents;
using WUG.BehaviorTreeVisualizer;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyBehaviorTrees.Nodes
{
    /// <summary>
    /// Tries to chase the player by checking for the player in a radius, calculating a path to the player, then moving along its calculated path at a rate of
    /// (1 / recalculateTimeSecs) times a second.
    /// </summary>
    public class ChasePlayer : Node
    {
        private float recalculateTimeSecs;
        private float timer = 0f;
        private IHostile context;
        private NPCAgentBase agent;
        
        public ChasePlayer(float recalculateTimeSecs, IHostile context)
        {
            Name = "Chase Player";
            this.recalculateTimeSecs = recalculateTimeSecs;
            this.context = context;
            agent = context.agent;
        }
        
        protected override void OnReset() {}

        protected override NodeStatus OnRun()
        {
            NavMeshPath path = new NavMeshPath();
            
            if (EvaluationCount == 0)
            {
                // First time this node runs, immediately start the calculation process below.
                timer = recalculateTimeSecs;
            }
            
            if (timer >= recalculateTimeSecs)
            {
                timer = 0f;

                GameObject player = context.TryGetPlayerWithinRange(context.playerAggroRange);
                
                // Recalculate the NavMeshPath if player was found.
                if (player != null)
                {
                    StatusReason = $"Successfully found the player within {context.playerAggroRange} units." +
                                   $"\n - Calculating path to the player.";
                    agent.MyNavMesh.CalculatePath(player.transform.position, path);

                    agent.MyNavMesh.SetPath(path);
                    
                    StatusReason = $"Set path to the player. Moving there now.";

                    return NodeStatus.RUNNING;
                }

                StatusReason = $"Could not find player within {context.playerAggroRange} units.";
                return NodeStatus.FAILURE;
            }
            
            // TODO: Find out why the node isn't repeating when RUNNING is returned.
            
            StatusReason = $"Currently chasing player." + 
                           $"\n - Current time to recalculate path: {recalculateTimeSecs - timer}.";
            return NodeStatus.RUNNING;
        }
    }
}
