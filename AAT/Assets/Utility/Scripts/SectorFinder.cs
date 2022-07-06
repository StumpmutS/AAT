using UnityEngine;

namespace Utility.Scripts
{
    public static class SectorFinder
    {
        public static SectorController FindSector(Vector3 point, float searchRadius, LayerMask searchLayers)
        {
            Collider[] sectors = new Collider[10];
            Physics.OverlapSphereNonAlloc(point, searchRadius, sectors, searchLayers);
            return DistanceCompare.FindClosestThing(sectors, point)?.GetComponent<SectorController>();
        }
    }
}
