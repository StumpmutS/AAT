using System.Linq;
using UnityEngine;

namespace Utility.Scripts
{
    public static class DistanceCompare
    {
        public static T FindClosestThing<T>(T[] things, Vector3 fromPoint, T returnIfPresent = default, T ignore = default) where T: Component
        {
            if (returnIfPresent != default)
                if (things.Contains(returnIfPresent)) return returnIfPresent;
            
            T target = default;
            var targetDistanceSquared = Mathf.Infinity;
            foreach(var thing in things)
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
