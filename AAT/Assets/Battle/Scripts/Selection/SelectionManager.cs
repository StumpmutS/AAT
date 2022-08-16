using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    public HashSet<SelectableController> Selectables { get; private set; } = new();
    public HashSet<SelectableController> Selected { get; private set; } = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddSelectable(SelectableController selectable)
    {
        Selectables.Add(selectable);
    }

    public void RemoveSelectable(SelectableController selectable)
    {
        Selectables.Remove(selectable);
    }

    public void AddSelected(SelectableController selectable)
    {
        Selected.Add(selectable);
        UIManager.Instance.ActivatePanel(selectable);
    }

    public void RemoveSelected(SelectableController selectable)
    {
        Selected.Remove(selectable);
        UIManager.Instance.DeactivatePanel(selectable);
    }
}
