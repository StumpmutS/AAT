using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableController : StumpEntity
{
    public event Action OnSelect = delegate { };
    public event Action OnDeselect = delegate { };
    public event Action OnHover = delegate { };
    public event Action OnHoverStop = delegate { };

    protected bool _selected;
    private bool _mouseOver;
    private bool _selectSubscribed;
    private bool _deselectSubscribed;

    protected virtual void OnMouseEnter()
    {
        _mouseOver = true;
        OnHover.Invoke();
        SubscribeSelect();
        UnsubscribeDeselect();
    }

    protected virtual void OnMouseExit()
    {
        _mouseOver = false;
        OnHoverStop.Invoke();
        UnsubscribeSelect();
        if (_selected) SubscribeDeselect();
    }

    public void CallSelect()
    {
        if (_selected || StumpEventSystemManagerReference.Instance.OverUI()) return;
        Select();
    }

    public void CallSelectOverrideUICheck()
    {
        if (_selected) return;
        Select();
    }

    protected virtual void Select()
    {
        _selected = true;
        OnSelect.Invoke();
        if (_mouseOver) return;
        UnsubscribeSelect();
        SubscribeDeselect();
    }

    public void CallDeselect()
    {
        if (!_selected || StumpEventSystemManagerReference.Instance.OverUI()) return;
        Deselect();
    }
    
    protected virtual void Deselect()
    {
        _selected = false;
        OnDeselect.Invoke();
        UnsubscribeDeselect();
    }

    private void SubscribeSelect()
    {
        if (_selectSubscribed) return;
        _selectSubscribed = true;
        InputManager.OnLeftCLickUp += CallSelect;
    }

    private void UnsubscribeSelect()
    {
        if (!_selectSubscribed) return;
        _selectSubscribed = false;
        InputManager.OnLeftCLickUp -= CallSelect;
    }
    
    private void SubscribeDeselect() 
    {
        if (_deselectSubscribed) return;
        _deselectSubscribed = true;
        InputManager.OnLeftCLickUp += CallDeselect;
    }
    
    private void UnsubscribeDeselect() 
    {
        if (!_deselectSubscribed) return;
        _deselectSubscribed = false;
        InputManager.OnLeftCLickUp -= CallDeselect;
    }
}
