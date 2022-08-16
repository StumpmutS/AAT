using System;
using UnityEngine;

public class SelectableController : MonoBehaviour
{
    [SerializeField] private ESelectionType selectionType;
    public ESelectionType SelectionType => selectionType;
    
    public event Action OnSelect = delegate { };
    public event Action OnDeselect = delegate { };
    public event Action OnHover = delegate { };
    public event Action OnHoverStop = delegate { };

    public bool Selected { get; private set; }
    private bool _mouseOver;
    private bool _selectSubscribed;
    private bool _deselectSubscribed;

    private void Start()
    {
        SelectionManager.Instance.AddSelectable(this);
    }

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
        if (Selected) SubscribeDeselect();
    }

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
        SelectionManager.Instance.AddSelected(this);
        Selected = true;
        OnSelect.Invoke();
        if (_mouseOver) return;
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
        SelectionManager.Instance.RemoveSelected(this);
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
        SelectionManager.Instance.RemoveSelectable(this);
    }
}
