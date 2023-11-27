using UnityEditor.Experimental.GraphView;

namespace EnemyBehaviorTrees.Internal
{
    public class BlackboardVariableId : IBlackboardVariableId
    {
        public string name { get; }

        public BlackboardVariableId(string name)
        {
            this.name = name;
        }
    }
}