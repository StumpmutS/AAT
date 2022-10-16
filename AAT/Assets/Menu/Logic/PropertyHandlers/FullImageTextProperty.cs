using System;
using System.Collections.Generic;
using UnityEngine;

public class FullImageTextProperty : MonoBehaviour
{
    [SerializeField] private FullImageTextDisplay display;
    [SerializeField] private PropertyHandler propertyHandler;
    public void Init(List<StylizedTextImage> icon, string label, Action<object> callback, object callbackObj)
    {
        display.SetStylizedImages(icon);
        display.SetText(label);
        propertyHandler.SetCallback(callback, callbackObj);
    }
}