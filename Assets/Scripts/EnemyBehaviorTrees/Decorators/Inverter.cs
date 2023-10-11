using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;

namespace EnemyBehaviorTrees.Decorators
{
    // Inverter Decorators invert whatever status they receive from their child node, like a NOT logic gate.
    
    public class Inverter : Decorator
    {
        // Constructor
        public Inverter(string displayName, Node node) : base(displayName, node) {}
        
        // OnRun() - return inverted NodeStatus.Success for NodeStatus.Failure and vice-versa
        protected override NodeStatus OnRun()
        {
            // Confirm that there's a valid child node that was passed
            if (ChildNodes.Count == 0 || ChildNodes[0] == null)
            {
                return NodeStatus.FAILURE;
            }
            
            // Run child
            NodeStatus childStatus = (ChildNodes[0] as Node).Run();

            // Evaluate child node
            switch (childStatus)
            {
                case NodeStatus.FAILURE:
                    return NodeStatus.SUCCESS;
                case NodeStatus.SUCCESS:
                    return NodeStatus.FAILURE;
            }
            
            // If it hit this point, NodeStatus is Running, return the same thing
            return childStatus;
        }
        
        // OnReset() - nothing to reset
        protected override void OnReset() {}
    }
}

