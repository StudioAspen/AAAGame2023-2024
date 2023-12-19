using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;

// Run the child node a certain amount of times
public class Repeater : Decorator
{
    private int timesToRun;

    public Repeater(string name, Node node, int timesToRun = 0) : base(name, node)
    {
        this.timesToRun = timesToRun;
    }

    protected override void OnReset() {}

    protected override NodeStatus OnRun()
    {
        // reference check
        if (timesToRun <= 0 || ChildNodes.Count == 0 || ChildNodes[0] == null)
        {
            return NodeStatus.FAILURE;
        }
        
        // update child
        NodeStatus returnStatus = (ChildNodes[0] as Node).Run();

        // stop if this is a count-limited repeat
        if (timesToRun > 0 && timesToRun == EvaluationCount)
        { 
            return NodeStatus.FAILURE; 
        }

        // otherwise return child state
        if (returnStatus == NodeStatus.RUNNING)
        { 
            return NodeStatus.SUCCESS; 
        }

        // finally reset
        Reset();
        (ChildNodes[0] as Node).Reset();
            
        return NodeStatus.SUCCESS;
    }
}
