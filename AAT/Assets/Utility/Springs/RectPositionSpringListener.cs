using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectPositionSpringListener : Vector3SpringListener
{
    [SerializeField] private RectTransform targetRect;

    protected override Vector3 GetOrig()
    {
        return targetRect.anchoredPosition;
    }

    protected override void ChangeValue(Vector3 value)
    {
        targetRect.anchoredPosition = value;
    }
}
