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
                target = DistanceCompare.FindClosestComponent(hits, pos, returnIfPresent, ignore);
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
                target = DistanceCompare.FindClosestComponent(hits.Select(h => h.Hitbox), pos, returnIfPresent, ignore);
                return true;
            }

            target = null;
            return false;
        }

        public static StumpTarget CheckHitPosition(NetworkRunner runner, PlayerRef inputAuthority, Vector3 position, Vector3 direction)
        {
            if (runner.LagCompensation.Raycast(position - direction.normalized * 50,
                    direction, 50.1f, inputAuthority, out var hit))
            {
                return TargetHelper.ConvertToStumpTarget(hit);
            }
        
            //Check for ground hit if no network hitboxes hit
            if (!Physics.Raycast(position - direction.normalized * 50,
                    direction, out var localHit, 50.1f, LayerManager.Instance.GroundLayer))
            {
                //check down from point if nothing found on direction
                Physics.Raycast(position, Vector3.down,
                    out localHit, position.y + 1, LayerManager.Instance.GroundLayer);
            }
        
            return TargetHelper.ConvertToStumpTarget(localHit);
        }
    }
}
