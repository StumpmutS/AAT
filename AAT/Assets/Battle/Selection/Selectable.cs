using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    [SerializeField] private ESelectionType selectionType;
    public ESelectionType SelectionType => selectionType;
    [SerializeField] private InputAwaiter inputAwaiter;
    public InputAwaiter InputAwaiter => inputAwaiter;

    public UnityEvent OnSelect;
    public UnityEvent OnDeselect;

    public bool Selected { get; private set; }
    public bool MouseOver { get; private set; }

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
    }
}