using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] private Outline outline;

    public event Action OnSelect = delegate { };
    public event Action OnDeselect = delegate { };
    public event Action OnOutline = delegate { };
    public event Action OnRemoveOutline = delegate { };
    public event Action OnHover = delegate { };
    public event Action OnHoverStop = delegate { };
    
    private bool selected = false;
    private bool outlined = false;
    private bool mouseOver = false;
    
    private void OnMouseEnter()
    {
        mouseOver = true;
        Outline();
        OnHover.Invoke();
        InputManager.OnLeftCLick += Select;
        InputManager.OnLeftCLick -= Deselect;
        InputManager.OnMinus += TestDamage;
    }

    private void TestDamage()
    {
        GetComponent<IHealth>().ModifyHealth(-100f);
    }

    private void OnMouseExit()
    {
        mouseOver = false;
        RemoveOutline();
        OnHoverStop.Invoke();
        InputManager.OnLeftCLick -= Select;
        InputManager.OnLeftCLick += Deselect;
        InputManager.OnMinus -= TestDamage;
    }

    public virtual void Select()
    {
        if (selected) return;
        Outline();
        selected = true;
        OnSelect.Invoke();
        if (!mouseOver)
        {
            InputManager.OnLeftCLick -= Select;
            InputManager.OnLeftCLick += Deselect;
        }
    }

    public virtual void Deselect()
    {
        if (!selected) return;
        selected = false;
        OnDeselect.Invoke();
        RemoveOutline();
        InputManager.OnLeftCLick -= Deselect;
    }
    
    public void Outline()
    {
        if (outline == null || outlined) return;

        outlined = true;
        outline.enabled = true;
        OnOutline.Invoke();
    }
    
    public void RemoveOutline()
    {
        if (selected || outline == null || !outlined) return;

        outlined = false;
        outline.enabled = false;
        OnRemoveOutline.Invoke();
    }
}
