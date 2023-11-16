using System.Collections;
using EnemyBehaviorTrees;
using UnityEngine;
using UnityEngine.AI;
using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees.Nodes;
using UnityEngine.UI;

namespace EnemyBehaviorTrees.Agents
{
    public abstract class NPCAgentBase : MonoBehaviour
    {
        public enum NavigationActivity
        {
            IDLE,
            LOOK_FOR_PLAYER,
            CHASE,
            PATROL
        }
        
        public NavMeshAgent MyNavMesh { get; private set; }
        public NavigationActivity MyActivity { get; set; }
        public NodeBase BehaviorTree { get; set; }
        public float playerCheckDistance;
        public int index; // Index of NPC in blackboard NPCs[]. Assigned automatically by Game Manager
    
        private Coroutine behaviorTreeRoutine;
        private YieldInstruction waitTime = new WaitForSeconds(.1f);
        private NPCAgentBlackboard blackboard;
    
        private void Start()
        {
            MyNavMesh = GetComponent<NavMeshAgent>();
            MyActivity = NavigationActivity.LOOK_FOR_PLAYER;
            blackboard = GameObject.Find("NPC Blackboard").GetComponent<NPCAgentBlackboard>();
            if (blackboard == null) { Debug.Log("Please create a blackboard Game Object called 'NPC Blackboard' with an 'NPCAgentBlackboard' component on it."); }
    
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
