using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineSelectableController : SelectableController
{
    [SerializeField] private Outline outline;

    public event Action OnOutline = delegate { };
    public event Action OnRemoveOutline = delegate { };
    
    private bool outlined = false;
    
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

    public override void Select()
    {
        base.Select();
        Outline();
    }

    public override void Deselect()
    {
        base.Deselect();
        RemoveOutline();
    }
    
    public void Outline()
    {
        if (outlined) return;
        outlined = true;
        outline.enabled = true;
        OnOutline.Invoke();
    }
    
    public void RemoveOutline()
    {
        if (selected || !outlined) return;

        outlined = false;
        outline.enabled = false;
        OnRemoveOutline.Invoke();
    }
}
