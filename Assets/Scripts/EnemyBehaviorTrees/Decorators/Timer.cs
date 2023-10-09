using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;
using UnityEngine;

namespace EnemyBehaviorTrees.Decorators
{
    // Timer Decorators run a node after a set amount of time expires
    
    public class Timer : Decorator
    {
        private float startTime;
        private bool useFixedTime;
        private float timeToWait;
        
        // Constructor
        public Timer(float timeToWait, Node childNode,  bool useFixedTime = false) : 
            base($"Timer for {timeToWait}", childNode) 
        {
            this.useFixedTime = useFixedTime;
            this.timeToWait = timeToWait;
        }
        
        // OnReset() - nothing to reset
        protected override void OnReset() {}
        
        // OnRun() - run after set time has passed      
        protected override NODE_STATUS OnRun()
        {
            // Confirm that a valid child node was passed in the constructor
            if (ChildNodes.Count == 0 || ChildNodes[0] == null)
            {
                return NODE_STATUS.Failure;
            }

            // Run the child node and calculate the elapsed
            NODE_STATUS originalStatus = (ChildNodes[0] as Node).Run();

            // If this is the first eval, then the start time needs to be set up.
            if (EvaluationCount == 0)
            {
                StatusReason = $"Starting timer for {timeToWait}. Child node status is: {originalStatus}";
                startTime = useFixedTime ? Time.fixedTime : Time.time;
            }

            // Calculate how much time has passed
            float elapsedTime = Time.fixedTime - startTime;

            // If more time has passed than we wanted, it's time to stop
            if (elapsedTime > timeToWait)
            {
                StatusReason = $"Timer complete - Child node status is: {originalStatus}";
                return NODE_STATUS.Success;
            }

            // Otherwise, keep running
            StatusReason = $"Timer is {elapsedTime} out of {timeToWait}. Child node status is: {originalStatus}";
            return NODE_STATUS.Running;

        }
    }
}

