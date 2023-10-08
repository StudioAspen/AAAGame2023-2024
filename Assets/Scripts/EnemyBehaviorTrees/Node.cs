using WUG.BehaviorTreeVisualizer;

// This is the base abstract superclass for all nodes in a behavior tree - it includes:
/*
 * public int EvaluationCount - how many times this node has been evaluated in a single run of it
 * public virtual NodeStatus Run() - overridable function to run any node's custom logic
 * public void Reset() - resets the node for another new run -- used at the end of all its logic
 * protected abstract NodeStatus OnRun() - provides any custom logic for when node is run (this is what we'll override to provide custom logic)
 * protected abstract void OnReset() - provides any custom logic for when node is reset
 */

// Node class inherits:
/*
 * public enum NodeStatus - the status of the current node with signatures: Failure, Success, Running, Unknown, and NotRun. We will use these within the code to determine what to do next within nodes.
 * public string Name - the name of the current node.
 * public string StatusReason - the reason for the current node's NodeStatus.
 * public List<NodeBase> ChildNodes - a linked list of child nodes connected to this one.
 * public NodeStatus LastNodeStatus - last node status
 */
public abstract class Node : NodeBase
{
    // Keeps track of the number of times the node has been evaluated in a single 'run'.
    public int EvaluationCount;
    
    // Runs the logic for the node
    public virtual NodeStatus Run()
    {
        // Runs the 'custom' logic
        NodeStatus nodeStatus = OnRun();
        
        // Increments the tracker for how many times the node has been evaluated this 'run'
        EvaluationCount++;
        
        // If the nodeStatus is not Running, then it is Success or Failure and can be Reset
        if (nodeStatus != NodeStatus.Running)
        {
            Reset();
        }
        
        // Return the StatusResult.
        return nodeStatus;
    }
    
    public void Reset()
    {
         EvaluationCount = 0;
         OnReset();
    }
    
    protected abstract NodeStatus OnRun();
    protected abstract void OnReset();
}