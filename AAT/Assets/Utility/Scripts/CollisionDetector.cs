using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Utility.Scripts
{
    public static class CollisionDetector
    {
        public static bool CheckRadius(Vector3 pos, float range, LayerMask layerMask, out Collider target, int maxHits = 50, Collider returnIfPresent = null, Collider ignore = null)
        {
            if (Physics.CheckSphere(pos, range, layerMask))
            {
                var hits = new Collider[maxHits];
                Physics.OverlapSphereNonAlloc(pos, range, hits, layerMask);
                target = DistanceCompare.FindClosestThing(hits, pos, returnIfPresent, ignore);
                return true;
            }

            target = null;
            return false;
        }
        
        public static bool CheckRadius(NetworkRunner runner, PlayerRef inputAuthority, Vector3 pos, float range,
            LayerMask layerMask, out Hitbox target, Hitbox returnIfPresent = default, Hitbox ignore = default)
        {
            var hits = new List<LagCompensatedHit>();
            if (runner.LagCompensation.OverlapSphere(pos, range, inputAuthority, hits, layerMask) > 0)
            {
                target = DistanceCompare.FindClosestThing(hits.Select(h => h.Hitbox), pos, returnIfPresent, ignore);
                return true;
            }

            target = null;
            return false;
        }
    }
}
