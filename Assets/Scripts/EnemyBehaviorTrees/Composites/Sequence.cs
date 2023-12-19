using EnemyBehaviorTrees;
using WUG.BehaviorTreeVisualizer;

namespace EnemyBehaviorTrees.Nodes
{
    // Sequence Composite node runs all children from left to right until either all children have run sucessfully
    // or at least one of them have returned NodeStatus.Failure
    public class Sequence : Composite
    {
        // Constructor
        public Sequence(string displayName, params Node[] childNodes) : base(displayName, childNodes) {}

        // OnRun() - contains logic to run every child node sequentially until one has returned Failure
        protected override NodeStatus OnRun()
        {
            // Check the status of the current child
            NodeStatus childNodeStatus = (ChildNodes[CurrentChildIndex] as Node).Run();
            
            // Evaluate the status. If failed, return to last node in tree with Failure
            switch (childNodeStatus)
            {
                // Failed, return failure
                case NodeStatus.FAILURE:
                    return childNodeStatus;
                // Succeeded, run next child
                case NodeStatus.SUCCESS:
                    CurrentChildIndex++;
                    break;
            }
            
            // All children have run successfully, return success
            if (CurrentChildIndex >= ChildNodes.Count)
            {
                return NodeStatus.SUCCESS;
            }
            
            // The child was a success but we may have more children to run, so call this method again or return that it's running.
            return childNodeStatus == NodeStatus.SUCCESS ? OnRun() : NodeStatus.RUNNING;
        }

        // Reset all nodes and index
        protected override void OnReset()
        {
            CurrentChildIndex = 0;
            
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                (ChildNodes[i] as Node).Reset();
            }
        }
    }
}
