using EnemyBehaviorTrees;
using WUG.BehaviorTreeVisualizer;

namespace EnemyBehaviorTrees.Composites
{
    // Sequence Composite node runs all children from left to right until either all children have run sucessfully
    // or at least one of them have returned NodeStatus.Failure
    public class Sequence : Composite
    {
        // Constructor
        public Sequence(string displayName, params Node[] childNodes) : base(displayName, childNodes) {}

        // OnRun() - contains logic to run every child node sequentially until one has returned Failure
        protected override NODE_STATUS OnRun()
        {
            // Check the status of the current child
            NODE_STATUS childNodeStatus = (ChildNodes[CurrentChildIndex] as Node).Run();
            
            // Evaluate the status. If failed, return to last node in tree with Failure
            switch (childNodeStatus)
            {
                // Failed, return failure
                case NODE_STATUS.Failure:
                    return childNodeStatus;
                // Succeeded, run next child
                case NODE_STATUS.Success:
                    CurrentChildIndex++;
                    break;
            }
            
            // All children have run successfully, return success
            if (CurrentChildIndex >= ChildNodes.Count)
            {
                return NODE_STATUS.Success;
            }
            
            // The child was a success but we may have more children to run, so call this method again or return that it's running.
            return childNodeStatus == NODE_STATUS.Success ? OnRun() : NODE_STATUS.Running;
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
