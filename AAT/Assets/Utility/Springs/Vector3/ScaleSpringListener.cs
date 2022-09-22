using System.Collections;
using UnityEngine;
using Utility.Scripts;

public class ScaleSpringListener : Vector3SpringListener
{
    [SerializeField] private Transform targetTransform;
    
    protected override Vector3 GetOrig()
    {
        return targetTransform.localScale;
    }

    protected override void ChangeValue(Vector3 value)
    {
        targetTransform.localScale = value.ClampZero();
    }
}