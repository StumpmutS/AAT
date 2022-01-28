using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DistanceCompare
{
    public static Collider FindClosestCollider(Collider[] colliders, Vector3 fromPoint, Collider current = null, Collider ignore = null)
    {
        Collider target = null;
        var targetDistanceSquared = Mathf.Infinity;
        foreach(var collider in colliders)
        {
            if (collider == null || collider == ignore) continue;
            if (collider == current) return current;

            var newDistanceSquared = (fromPoint - collider.transform.position).sqrMagnitude;
            if (!(newDistanceSquared < targetDistanceSquared)) continue;
            targetDistanceSquared = newDistanceSquared;
            target = collider;
        }
        return target;
    }
}
