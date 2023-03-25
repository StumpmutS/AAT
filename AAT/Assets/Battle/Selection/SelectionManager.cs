using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField] private SelectionPriority selectionPriority;

    public Dictionary<ESelectionType, HashSet<Selectable>> Selected { get; private set; } = new();
    public ESelectionType SelectedType => selectionPriority.GetHighestPriority(Selected.Keys);

    private HashSet<SelectionTarget> _allSelectionTargets = new();

    public event Action<IEnumerable<Selectable>> OnSelected = delegate { };
    public event Action<IEnumerable<Selectable>> OnDeselected = delegate { };

    public void AddSelectionTarget(SelectionTarget selectionTarget)
    {
        selectionTarget.OnSelect += HandleSelected;
        selectionTarget.OnDeselect += HandleDeselected;
        _allSelectionTargets.Add(selectionTarget);
    }

    public void RemoveSelectionTarget(SelectionTarget selectionTarget)
    {
        selectionTarget.OnSelect -= HandleSelected;
        selectionTarget.OnDeselect -= HandleDeselected;
        _allSelectionTargets.Remove(selectionTarget);
    }

    private void HandleSelected(Selectable selectable)
    {
        if (!Selected.ContainsKey(selectable.SelectionType))
        {
            Selected[selectable.SelectionType] = new HashSet<Selectable>();
        }

        Selected[selectable.SelectionType].Add(selectable);
        OnSelected.Invoke(new[] {selectable});
    }

    private void HandleDeselected(Selectable selectable)
    {
        if (Selected.ContainsKey(selectable.SelectionType))
        {
            Selected[selectable.SelectionType].Remove(selectable);
        }

        OnDeselected.Invoke(new[] {selectable});
    }
}