using System;
using System.Collections.Generic;

namespace EnemyBehaviorTrees.Internal
{
    public class Blackboard : IBlackboard
    {
        public Dictionary<string, object> entries { get; } = new Dictionary<string, object>();
        
        public bool SetEntry<T>(string id, T value)
        {
            // NOTE: Dictionary.TryAdd() doesn't work here because TryAdd doesn't override already added key-value pairs.
            
            // Eheck for entry in entries
            if (entries.ContainsKey(id))
            {
                // Check object types
                if (entries[id].GetType() != value.GetType())
                {
                    return false;
                }
                
                // Otherwise, add entry
                entries[id] = value;
            }
            // Entry not already in entries
            else
            {
                entries.Add(id, value);
            }

            return true;
        }

        public T GetEntry<T>(string key, out bool getEntrySuccess)
        {
            // Try to get the value at key and check if the output's value is the same type as the function's output value.
            if (entries.TryGetValue(key, out var objValue) && objValue is T)
            {
                getEntrySuccess = true;
                return (T)objValue;     // Not type safe
            }

            getEntrySuccess = false;
            return default;
        }

        public Dictionary<string, object> GetBlackboardEntries()
        {
            return entries;
        }
    }
}