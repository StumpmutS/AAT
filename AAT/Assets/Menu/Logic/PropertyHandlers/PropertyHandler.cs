using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PropertyHandler : MonoBehaviour
{
    protected Action<object> _propertyChangedCallback;
    protected object _callbackData;

    public void SetCallback(Action<object> callback, object callbackData)
    {
        _propertyChangedCallback = callback;
        _callbackData = callbackData;
    }
}
