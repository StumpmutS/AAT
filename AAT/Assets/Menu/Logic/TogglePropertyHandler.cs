using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TogglePropertyHandler : PropertyHandler
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private int toggleValue;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(HandleToggle);
    }

    public override void Init(GameObject go, UnityAction<int, StumpData> callback, StumpData callbackData)
    {
        base.Init(go, callback, callbackData);
        if (go.TryGetComponent<ToggleGroup>(out var toggleGroup))
        {
            toggle.group = toggleGroup;
        }
    }

    private void HandleToggle(bool value)
    {
        if (value) _propertyChangedCallback.Invoke(toggleValue, _callbackData);
    }
}
