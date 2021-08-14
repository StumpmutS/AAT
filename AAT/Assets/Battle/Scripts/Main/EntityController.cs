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
    
    private bool selected = false;
    
    private void OnMouseEnter()
    {
        Outline();
        InputManager.OnLeftCLick += Select;
        InputManager.OnJump += TestDamage;
    }

    private void TestDamage(float hehe)
    {
        GetComponent<IHealth>().ModifyHealth(-100f);
    }

    private void OnMouseExit()
    {
        RemoveOutline();
        InputManager.OnLeftCLick -= Select;
        InputManager.OnLeftCLick += Deselect;
        InputManager.OnJump -= TestDamage;
    }

    public virtual void Select()
    {
        OnSelect.Invoke();
        selected = true;
        Outline();
    }

    public virtual void Deselect()
    {
        OnDeselect.Invoke();
        selected = false;
        RemoveOutline();
        InputManager.OnLeftCLick -= Deselect;
    }
    
    public void Outline()
    {
        if (outline != null)
            outline.enabled = true;
        OnOutline.Invoke();
    }
    
    public void RemoveOutline()
    {
        if (selected) return;
        if (outline != null)
            outline.enabled = false;
        OnRemoveOutline.Invoke();
    }
}
