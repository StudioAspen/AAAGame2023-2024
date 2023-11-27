using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;


namespace EnemyBehaviorTrees.Internal
{
    public interface IBlackboard
    {
        Dictionary<string, object> entries { get; }
        
        /// <summary>
        /// Creates a a new entry &lt; IBlackboardVariableId id, object value &gt; in the Blackboard. If the given Id already exists on the Blackboard, the value will be overwritten if,
        /// and only if, the existing value type and provided type match.
        ///
        /// <p> <b> If types do not match, value won't be modified and the function will return false. </b> </p>
        ///
        /// <param name="id"> Id of the entry. </param>
        /// <typeparam name="value"> Value of the entry. WARNING: VALUE MUST BE OF A PRIMITIVE TYPE </typeparam>
        /// <returns> True if successfully set. </returns>
        /// </summary>
        public bool SetEntry(string id, object value);
        
        
        /// <summary>
        /// Gets the value of a entry by id if it exists on the Blackboard.
        ///
        /// <param name="id"> Id of the entry. </param>
        /// <returns> The object if Id exists. Otherwise, it returns null. </returns>
        /// </summary>
        public object GetEntry(string id);


        /// <summary>
        /// This method returns whether or not a given entry is in the Blackboard.
        /// </summary>
        /// <param name="id"> The id of the entry to search for. </param>
        /// <returns> <c>True</c> if the there is an entry in the Blackboard with the given id and <c>False</c> otherwise. </returns>
        public bool ContainsEntry(string id);
            
        
        /// <summary>
        /// Get the dictionary of blackboard entries in the blackboard.
        /// 
        /// <remarks> Only use whenever you need to iterate through all of the blackboard entries. If you need to retrieve a single entry, use GetVariable() instead. </remarks>
        ///
        /// <returns> <c>Dictionary&lt;string, object&gt;</c> of all entries in the current blackboard. </returns>
        /// </summary>
        public Dictionary<string, object> GetBlackboardEntries();
    }
}