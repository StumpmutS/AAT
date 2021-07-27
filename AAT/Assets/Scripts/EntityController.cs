using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] private Outline outline;

    public event Action OnSelect = delegate { };
    public event Action OnDeselect = delegate { };
    
    private bool selected = false;
    
    private void OnMouseEnter()
    {
        Outline();
        InputManager.OnLeftCLick += Select;
    }

    private void OnMouseExit()
    {
        RemoveOutline();
        InputManager.OnLeftCLick -= Select;
        InputManager.OnLeftCLick += Deselect;
    }

    private void Select()
    {
        OnSelect.Invoke();
        selected = true;
        Outline();
    }

    private void Deselect()
    {
        OnDeselect.Invoke();
        selected = false;
        RemoveOutline();
        InputManager.OnLeftCLick -= Deselect;
    }
    
    private void Outline()
    {
        if (outline != null)
            outline.enabled = true;
    }
    
    private void RemoveOutline()
    {
        if (selected) return;
        if (outline != null)
            outline.enabled = false;
    }
}
