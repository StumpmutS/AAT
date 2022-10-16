using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TogglePropertyHandler : PropertyHandler
{
    [SerializeField] private Toggle toggle;
    public Toggle Toggle => toggle;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(HandleToggle);
    }

    private void HandleToggle(bool value)
    {
        if (value) _propertyChangedCallback.Invoke(_callbackData);
    }
}