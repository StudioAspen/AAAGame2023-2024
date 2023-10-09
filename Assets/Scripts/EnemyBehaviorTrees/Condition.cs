using WUG.BehaviorTreeVisualizer;
using EnemyBehaviorTrees;

namespace EnemyBehaviorTrees
{
    // Base superclass for Condition nodes. Condition nodes are nodes that test a condition in the game in order to choose which child to execute.
    // e.g. how far is the player from this agent? if x units away, chase, if y units away, attack, things like that.
    
    // This code just allows the Behavior Tree Visualizer to stylize all conditions a specific way
    public abstract class Condition : Node
    {
        public Condition(string name)
        {
            Name = name;
        }
    }
}
