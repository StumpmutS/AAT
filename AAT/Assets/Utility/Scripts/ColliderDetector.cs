using UnityEngine;

namespace Utility.Scripts
{
    public static class ColliderDetector
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
    }
}
