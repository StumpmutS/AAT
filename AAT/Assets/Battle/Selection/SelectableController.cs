using System;
using UnityEngine;
using UnityEngine.Events;

public class SelectableController : MonoBehaviour
{
    [SerializeField] private ESelectionType selectionType;
    public ESelectionType SelectionType => selectionType;
    
    public UnityEvent OnSelect;
    public UnityEvent OnDeselect;

    public bool Selected { get; private set; }
    public bool MouseOver { get; private set; }
    private bool _selectSubscribed;
    private bool _deselectSubscribed;

    public void CallSelect()
    {
        if (Selected || UIHoveredReference.Instance.OverUI()) return;
        Select();
    }

    public void CallSelectOverrideUICheck()
    {
        if (Selected) return;
        Select();
    }

    protected virtual void Select()
    {
        Selected = true;
        OnSelect.Invoke();
        if (MouseOver) return;
        UnsubscribeSelect();
        SubscribeDeselect();
    }

    public void CallDeselect()
    {
        if (!Selected || UIHoveredReference.Instance.OverUI()) return;
        Deselect();
    }

    public void CallDeselectOverrideUICheck()
    {
        if (!Selected) return;
        Deselect();
    }
    
    protected virtual void Deselect()
    {
        Selected = false;
        OnDeselect.Invoke();
        UnsubscribeDeselect();
    }

    private void SubscribeSelect()
    {
        if (_selectSubscribed) return;
        _selectSubscribed = true;
        BaseInputManager.OnLeftCLickUp += CallSelect;
    }

    private void UnsubscribeSelect()
    {
        if (!_selectSubscribed) return;
        _selectSubscribed = false;
        BaseInputManager.OnLeftCLickUp -= CallSelect;
    }
    
    private void SubscribeDeselect() 
    {
        if (_deselectSubscribed) return;
        _deselectSubscribed = true;
        BaseInputManager.OnLeftCLickUp += CallDeselect;
    }
    
    private void UnsubscribeDeselect() 
    {
        if (!_deselectSubscribed) return;
        _deselectSubscribed = false;
        BaseInputManager.OnLeftCLickUp -= CallDeselect;
    }

    private void OnDestroy()
    {
        BaseInputManager.OnLeftCLickUp -= CallSelect;
        BaseInputManager.OnLeftCLickUp -= CallDeselect;
    }
}
