using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StylizedImageToggleProperty : MonoBehaviour
{
    [SerializeField] private FullImageTextDisplay display;
    [SerializeField] private TogglePropertyHandler togglePropertyHandler;
    public Toggle Toggle => togglePropertyHandler.Toggle;

    public void Init(List<StylizedTextImage> images, Action<object> callback, object callbackObj)
    {
        display.SetStylizedImages(images);
        togglePropertyHandler.SetCallback(callback, callbackObj);
    }
}