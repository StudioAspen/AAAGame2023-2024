using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WUG.BehaviorTreeVisualizer;

namespace EnemyBehaviorTrees.Internal
{
    public abstract class NPCAgentBase : MonoBehaviour, IBehaviorTree
    {
        // Add any new Navigation Activities you have a need for here, we can't really generalize these in the interfaces like I wanted to, because interfaces
        // can't contain enums. :(
        public enum NavigationActivity
        {
            IDLE,
            CHASE,
            PATROL,
            LOOK_FOR_PLAYER,
        }
        
        public NavMeshAgent MyNavMesh { get; private set; }
        public NodeBase BehaviorTree { get; set; }
        public Blackboard blackboard = new Blackboard();
    
        private Coroutine behaviorTreeRoutine;
        private YieldInstruction waitTime = new WaitForSeconds(.1f);
    
        private void Start()
        {
            MyNavMesh = this.GetComponent<NavMeshAgent>();
            
            // Set default navigation activity
            blackboard.SetEntry<NavigationActivity>("Navigation Activity", NavigationActivity.LOOK_FOR_PLAYER);
            
            Debug.Log($"Initialized {this.name} with Navigation activity of: {blackboard.GetEntry<NavigationActivity>("Navigation Activity", out bool result).ToString()}");
    
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
    }
}
