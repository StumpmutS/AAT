using System.Collections.Generic;
using UnityEngine;

namespace Utility.Scripts
{
    public static class StumpDictionaryExtensions
    {
        public static K MinKeyByValue<K>(IDictionary<K, float> dictionary)
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
    }
}
