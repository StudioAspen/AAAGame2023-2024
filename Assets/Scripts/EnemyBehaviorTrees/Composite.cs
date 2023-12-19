using System.Linq;

namespace EnemyBehaviorTrees
{
    // This is the base superclass for all Composite nodes in a behavior tree 
    // A Composite node is a node that provides a guideline to its child nodes for how they should run
    // e.g. A Sequence Composite node attempts to run each of their child nodes in a sequence from left to right until one of them returns a 
    // NodeStatus of Failure.
    
    
    public abstract class Composite : Node
    {
        // Current index of child that is running
        protected int CurrentChildIndex = 0;
        
        // Constructor
        // Params keyword allows us to pass nodes in with comma-separated values if required
        protected Composite(string displayName, params Node[] childNodes)
        {
            Name = displayName;
            
            ChildNodes.AddRange(childNodes.ToList());
        }
    }
}
