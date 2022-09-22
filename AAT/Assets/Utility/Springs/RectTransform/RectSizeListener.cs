using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectSizeListener : Vector3SpringListener
{
    [SerializeField] private RectTransform rectTransform;
    
    protected override Vector3 GetOrig()
    {
        return rectTransform.sizeDelta;
    }

    protected override void ChangeValue(Vector3 value)
    {
        rectTransform.sizeDelta = value;
    }
}
