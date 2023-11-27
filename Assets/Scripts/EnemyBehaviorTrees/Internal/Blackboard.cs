using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.XR;

namespace EnemyBehaviorTrees.Internal
{
    public class Blackboard : IBlackboard
    {
        public Dictionary<string, object> entries { get; }
        
        public bool SetEntry(string id, object value)
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

        public object GetEntry(string id)
        {
            // NOTE: Same deal with Dictionary.TryGetValue() as SetEntry().
            
            // check for entry in entries
            if (entries.ContainsKey(id))
            {
                return entries[id];
            }
            // entries doesn't contain id
            else
            {
                return null;
            }
        }
        
        public bool ContainsEntry(string id)
        {
            return entries.ContainsKey(id);
        }

        public Dictionary<string, object> GetBlackboardEntries()
        {
            return entries;
        }
    }
}