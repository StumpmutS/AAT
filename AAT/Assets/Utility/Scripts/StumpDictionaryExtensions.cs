using System.Collections.Generic;
using UnityEngine;

namespace Utility.Scripts
{
    public static class StumpDictionaryExtensions
    {
        public static K MinKeyByValue<K, T2>(Dictionary<K, (float, T2)> dictionary)
        {
            K key = default;
            var shortest = Mathf.Infinity;
            foreach (var kvp in dictionary)
            {
                if (!(kvp.Value.Item1 < shortest)) continue;
                shortest = kvp.Value.Item1;
                key = kvp.Key;
            }

            return key;
        }
    }
}
