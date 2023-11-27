using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WUG.BehaviorTreeVisualizer;

namespace EnemyBehaviorTrees.Internal
{
    public abstract class NPCAgentBase : MonoBehaviour, IBehaviorTree
    {
        public NavMeshAgent MyNavMesh { get; private set; }
        public NodeBase BehaviorTree { get; set; }
        public Blackboard blackboard = new Blackboard();
    
        private Coroutine behaviorTreeRoutine;
        private YieldInstruction waitTime = new WaitForSeconds(.1f);
    
        private void Start()
        {
            MyNavMesh = GetComponent<NavMeshAgent>();
            
            // set default navigation activity
            blackboard.SetEntry("Navigation Activity", "Look for player");
            
            Debug.Log($"Initialized NPC with Navigation activity of: {blackboard.GetEntry<string>("Navigation Activity", out string value)}");
    
            GenerateBehaviorTree();
    
            if (behaviorTreeRoutine == null && BehaviorTree != null)
            {
                behaviorTreeRoutine = StartCoroutine(RunBehaviorTree());
            }
        }
    
        public virtual void GenerateBehaviorTree() {}
    
        protected IEnumerator RunBehaviorTree()
        {
            while (enabled)
            {
                if (BehaviorTree == null)
                {
                    $"{this.GetType().Name} is missing Behavior Tree. Did you set the BehaviorTree property?".BTDebugLog();
                    continue;
                }
    
                (BehaviorTree as Node).Run();
    
                yield return waitTime;
            }
        }
    
        protected void OnDestroy()
        {
            if (behaviorTreeRoutine != null)
            {
                StopCoroutine(behaviorTreeRoutine);
            }
        }

        // These declarations are here so that we can consistently use NPCAgentBase as context in nodes.
        #region INTERFACE SPECIFIC ABSTRACT METHODS
        
        public abstract GameObject TryGetPlayerWithinRange(float range);
        public abstract GameObject GetNextWayPoint();
        public abstract GameObject GetRandomWayPoint();

        #endregion
    }
}
