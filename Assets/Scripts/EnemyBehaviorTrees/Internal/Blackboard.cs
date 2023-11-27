using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.XR;

namespace EnemyBehaviorTrees.Internal
{
    public class Blackboard : IBlackboard
    {
        public Dictionary<string, object> entries { get; } = new Dictionary<string, object>();
        
        public bool SetEntry<T>(string id, T value)
        {
            // NOTE: Dictionary.TryAdd() doesn't work here because TryAdd doesn't override already added key-value pairs.
            
            // check for entry in entries
            if (entries.ContainsKey(id))
            {
                // check object types
                if (entries[id].GetType() != value.GetType())
                {
                    return false;
                }
                
                // otherwise, add entry
                entries[id] = value;
            }
            // entry not already in entries
            else
            {
                entries.Add(id, value);
            }

            return true;
        }

        public bool GetEntry<T>(string key, out T value)
        {
            if (entries.TryGetValue(key, out var objValue) && objValue is T)
            {
                value = (T)objValue;
                return true;
            }

            value = default;
            return false;
        }

        public Dictionary<string, object> GetBlackboardEntries()
        {
            return entries;
        }
    }
}