using EnemyBehaviorTrees;
using WUG.BehaviorTreeVisualizer;

namespace EnemyBehaviorTrees.Composites
{
    // Selector Composite nodes attempt to run at least one of its nodes successfully. It will keep running child nodes from left to right until a Success is returned, then,
    // it doesn't run any other children and a NodeStatus.Success is returned. Otherwise, if no child node returns a Success, a Failure will be returned.
    
    public class Selector : Composite
    {
        public Selector(string displayName, params Node[] childNodes) : base(displayName, childNodes) {}
    
        protected override NODE_STATUS OnRun()
        {
            // We've reached the end of the ChildNodes and no one was successful
            if (CurrentChildIndex >= ChildNodes.Count)
            {
                return NODE_STATUS.Failure;
            }
            
            // Call the current child
            NODE_STATUS nodeStatus = (ChildNodes[CurrentChildIndex]as Node).Run();
            
            // Check the child's status - failure means try a new child, Success means done.
            switch (nodeStatus)
            {
                case NODE_STATUS.Failure:
                    CurrentChildIndex++;
                    break;
                case NODE_STATUS.Success:
                    return NODE_STATUS.Success;
            }
            
            // If this point as been hit - then the current child is still running
            return NODE_STATUS.Running;
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
