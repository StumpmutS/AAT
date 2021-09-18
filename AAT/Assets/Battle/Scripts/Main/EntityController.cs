using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public event Action OnSelect = delegate { };
    public event Action OnDeselect = delegate { };
    public event Action OnHover = delegate { };
    public event Action OnHoverStop = delegate { };

    protected bool selected = false;
    private bool mouseOver = false;

    protected virtual void OnMouseEnter()
    {
        mouseOver = true;
        OnHover.Invoke();
        InputManager.OnLeftCLick += Select;
        InputManager.OnLeftCLick -= Deselect;
        InputManager.OnMinus += TestDamage;
    }

    private void TestDamage()
    {
        var health = GetComponent<IHealth>();
        if (health != null)
            health.ModifyHealth(-100f);
    }

    protected virtual void OnMouseExit()
    {
        mouseOver = false;
        OnHoverStop.Invoke();
        InputManager.OnLeftCLick -= Select;
        InputManager.OnLeftCLick += Deselect;
        InputManager.OnMinus -= TestDamage;
    }

    public virtual void Select()
    {
        if (selected) return;
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
        InputManager.OnLeftCLick -= Deselect;
    }
}
