using System.Collections.Generic;
using UnityEngine;

namespace Utility.Scripts
{
    public static class StumpDictionaryExtensions
    {
        public static K MinKeyByValue<K>(this IEnumerable<KeyValuePair<K, float>> dictionary)
        {
            K key = default;
            var shortest = Mathf.Infinity;
            foreach (var kvp in dictionary)
            {
                if (!(kvp.Value < shortest)) continue;
                shortest = kvp.Value;
                key = kvp.Key;
            }

            return key;
        }
        
        public static K MinKeyByValue<K>(this IEnumerable<KeyValuePair<K, int>> dictionary)
        {
            K key = default;
            var shortest = Mathf.Infinity;
            foreach (var kvp in dictionary)
            {
                if (!(kvp.Value < shortest)) continue;
                shortest = kvp.Value;
                key = kvp.Key;
            }

            return key;
        }
        
        public static K MaxKeyByValue<K>(this IEnumerable<KeyValuePair<K, float>> dictionary)
        {
            K key = default;
            var longest = -Mathf.Infinity;
            foreach (var kvp in dictionary)
            {
                if (kvp.Value < longest) continue;
                longest = kvp.Value;
                key = kvp.Key;
            }

            return key;
        }
        
        public static K MaxKeyByValue<K>(this IEnumerable<KeyValuePair<K, int>> dictionary)
        {
            K key = default;
            var longest = -Mathf.Infinity;
            foreach (var kvp in dictionary)
            {
                if (kvp.Value < longest) continue;
                longest = kvp.Value;
                key = kvp.Key;
            }

            return key;
        }
    }
}
