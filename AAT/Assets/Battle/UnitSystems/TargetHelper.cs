using System;
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
        return hit.collider == null ? new StumpTarget(default, default) : new StumpTarget(hit.collider.gameObject, hit.point);
    }
    
    public static StumpTarget ConvertToStumpTarget(Component component)
    {
        return component == null ? new StumpTarget(null, Vector3.zero) : new StumpTarget(component.gameObject, component.transform.position);
    }

    public static StumpTarget ConvertToStumpTarget(NetworkedStumpTarget target)
    {
        if (StumpNetworkRunner.Instance.Runner.TryFindObject(target.Id, out var networkObject))
        {
            return new StumpTarget(networkObject.gameObject, target.Point);
        }

        return new StumpTarget(null, target.Point);
    }

    public static NetworkedStumpTarget ConvertToNetworkedStumpTarget(StumpTarget target)
    {
        if (target.Hit.TryGetComponent<NetworkObject>(out var networkObject))
        {
            return new NetworkedStumpTarget(networkObject.Id, target.Point);
        }
        
        return new NetworkedStumpTarget(default, target.Point);
    }
}