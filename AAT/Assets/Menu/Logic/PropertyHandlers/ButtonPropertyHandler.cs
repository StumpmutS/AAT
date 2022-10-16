using UnityEngine;
using UnityEngine.UI;

public class ButtonPropertyHandler : PropertyHandler
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        _propertyChangedCallback.Invoke(_callbackData);
    }
}