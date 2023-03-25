using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class VisualsListener : MonoBehaviour
{
    [Tooltip("Use if keeping decals outside of (mesh/other visuals) is needed")]
    [SerializeField] private float directionMultiplier;
    [SerializeField] private Vector3 offset;
    
    public void AcceptVisuals(Dictionary<VisualComponent, VisualInfo> visuals)
    {
        foreach (var (data, info) in visuals)
        {
            data.ActivateComponent(OverrideInfo(info));
        }
    }

    protected virtual VisualInfo OverrideInfo(VisualInfo info)
    {
        info.DirectionMultiplier = directionMultiplier;
        info.Position += offset;
        return info;
    }
}