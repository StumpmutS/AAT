using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PropertyHandler : MonoBehaviour
{
    [SerializeField] private Image image;
    public Image Image => image;
    
    protected UnityAction<int, StumpData> _propertyChangedCallback;
    protected StumpData _callbackData;

    public virtual void Init(GameObject go, UnityAction<int, StumpData> callback, StumpData callbackData)
    {
        _propertyChangedCallback = callback;
        _callbackData = callbackData;
    }
}
