using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility.Scripts
{
    public static class DistanceCompare
    {
        public static T FindClosestComponent<T>(IEnumerable<T> things, Vector3 fromPoint, T returnIfPresent = default, T ignore = default) where T: Component
        {
            var components = things as T[] ?? things.ToArray();
            if (returnIfPresent != default)
                if (components.Contains(returnIfPresent)) return returnIfPresent;
            
            T target = default;
            var targetDistanceSquared = Mathf.Infinity;
            foreach(var thing in components)
            {
                if (thing is null || thing.Equals(ignore)) continue;

                var newDistanceSquared = (fromPoint - thing.transform.position).sqrMagnitude;
                if (!(newDistanceSquared < targetDistanceSquared)) continue;
                targetDistanceSquared = newDistanceSquared;
                target = thing;
            }
            return target;
        }
    }
}
