using System;
using UnityEngine;

public class OutlineSelectableController : SelectableController
{
    [SerializeField] private OutlineController outline;

    public event Action OnOutline = delegate { };
    public event Action OnRemoveOutline = delegate { };
    
    private bool _outlined;
    
    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
        Outline();
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();
        RemoveOutline();
    }

    protected override void Select()
    {
        base.Select();
        Outline();
    }

    protected override void Deselect()
    {
        base.Deselect();
        RemoveOutline();
    }
    
    public void Outline()
    {
        if (_outlined) return;
        _outlined = true;
        outline.Activate();
        OnOutline.Invoke();
    }
    
    public void RemoveOutline()
    {
        if (Selected || !_outlined) return;

        _outlined = false;
        outline.Deactivate();
        OnRemoveOutline.Invoke();
    }
}
