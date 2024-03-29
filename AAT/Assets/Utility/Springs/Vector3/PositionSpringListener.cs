using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSpringListener : Vector3SpringListener
{
    [SerializeField] private Transform targetTransform;
    
    protected override Vector3 GetOrig()
    {
        return targetTransform.localPosition;
    }

    protected override void ChangeValue(Vector3 value)
    {
        targetTransform.localPosition = value;
    }
}
