using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

public class GroupScaleSpringListener : Vector3SpringListener
{
    [SerializeField] private List<Transform> targetTransforms;
    
    protected override Vector3 GetOrig()
    {
        return targetTransforms[0].localScale;
    }

    protected override void ChangeValue(Vector3 value)
    {
        foreach (var targetTransform in targetTransforms)
        {
            targetTransform.localScale = value.ClampZero();
        }
    }
}