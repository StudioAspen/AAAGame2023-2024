using EnemyBehaviorTrees;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Nodes;

namespace EnemyBehaviorTrees.Nodes
{
    public class AttackPlayer : Node
    {
        public AttackPlayer()
        {
            Name = "Attack player";
        }

        protected override void OnReset() {}

        protected override NodeStatus OnRun()
        {
            // UNIMPLEMENTED
            return NodeStatus.SUCCESS;
        }
    }
}
