using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;

namespace EnemyBehaviorTrees
{
    // This is the base abstract class for Decorator nodes. Decorator nodes are nodes that alter the behavior of an instance of a class. Decorators only ever have one child.
    // e.g. The Inverter Decorator takes whatever is returned by another node and inverts it, like a NOT logic gate.
    
    public abstract class Decorator : Node
    {
        // Constructor - only difference between this and Composites is that there's only one child node.
        public Decorator(string displayName, Node node)
        {
            Name = displayName;
            ChildNodes.Add(node);
        }
    }
}

