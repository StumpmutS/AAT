using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleEFaction : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private EFaction faction;

    public UnityEvent<EFaction> OnToggleValueChanged;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(HandleValueChanged);
    }

    private void HandleValueChanged(bool value)
    {
        if (value) OnToggleValueChanged.Invoke(faction);
    }
    
    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(HandleValueChanged);
    }
}