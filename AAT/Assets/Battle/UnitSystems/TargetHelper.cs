using Fusion;
using UnityEngine;

public static class TargetHelper
{
    public static StumpTarget ConvertToStumpTarget(LagCompensatedHit hit)
    {
        return new StumpTarget(hit.GameObject, hit.Point);
    }
    
    public static StumpTarget ConvertToStumpTarget(RaycastHit hit)
    {
        return new StumpTarget(hit.collider.gameObject, hit.point);
    }
    
    public static StumpTarget ConvertToStumpTarget(Component component)
    {
        return new StumpTarget(component.gameObject, component.transform.position);
    }
}