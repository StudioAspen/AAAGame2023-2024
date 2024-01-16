using System.Collections;
using System.Collections.Generic;


namespace EnemyBehaviorTrees.Internal
{
    public interface IBlackboard
    {
        /// <summary>
        /// <p>The actual blackboard object itself. This is where we will be assigning entries to string name values.</p>
        /// <p>One problem with this approach though, is that because we are using the "object" keyword, the dictionary only works with primitive (string, int, float, ect.)
        /// types. This is possibly not good because this means we can't use the blackboard to store UnityEngine types such as GameObjects or Components.</p>
        ///
        /// <p>I'm still thinking of a workaround at the moment.</p>
        /// </summary>
        Dictionary<string, object> entries { get; }
        
        /// <summary>
        /// Creates a a new entry &lt;String id, object value&gt; in the Blackboard. If the given Id already exists on the Blackboard, the value will be overwritten if,
        /// and only if, the existing value type and provided type match.
        ///
        /// <p> <b> If types do not match, value won't be modified and the function will return false. </b> </p>
        ///
        /// <param name="id"> Id of the entry. </param>
        /// <typeparam name="value"> Value of the entry. WARNING: VALUE MUST BE OF A PRIMITIVE TYPE </typeparam>
        /// <returns> True if successfully set. </returns>
        /// </summary>
        public bool SetEntry<T>(string id, T value);
        
        
        /// <summary>
        /// Gets the value of a entry by id if it exists on the Blackboard.
        ///
        /// <param name="id"> Id of the entry. </param>
        /// <param name="getEntrySuccess"> Whether or not the value was successfully gotten. </param>
        /// <returns> The value that the id corresponds to if it exists. Otherwise, it returns the default value for the Type supplied. To circumvent this,
        /// check it against the getEntrySuccess boolean if you're uncertain if the key exists.</returns>
        /// </summary>
        public T GetEntry<T>(string id, out bool getEntrySuccess);
            
        
        /// <summary>
        /// Get the dictionary of blackboard entries in the blackboard.
        /// 
        /// <remarks> Only use whenever you need to iterate through all of the blackboard entries. If you need to retrieve a single entry, use GetEntry() instead. </remarks>
        ///
        /// <returns> <c>Dictionary&lt;string, object&gt;</c> of all entries in the current blackboard. </returns>
        /// </summary>
        public Dictionary<string, object> GetBlackboardEntries();
    }
}