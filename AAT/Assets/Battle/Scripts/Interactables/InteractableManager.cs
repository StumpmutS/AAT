using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager Instance { get; private set; }

    private HashSet<InteractableController> _interactables = new HashSet<InteractableController>();
    private int _unitSelectedCount;
    private InteractableController _hoveredInteractable;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UnitManager.Instance.OnUnitAdded += HandeUnitAdded;
        UnitManager.Instance.OnUnitRemoved += HandleUnitRemoved;
    }

    public void AddInteractable(InteractableController interactable)
    {
        _interactables.Add(interactable);
    }

    private void HandeUnitAdded(UnitController unit)
    {
        unit.OnSelect += HandleUnitSelected;
        unit.OnDeselect += HandleUnitDeselected;
    }

    private void HandleUnitRemoved(UnitController unit)
    {
        unit.OnSelect -= HandleUnitSelected;
        unit.OnDeselect -= HandleUnitDeselected;
    }

    private void HandleUnitSelected()
    {
        _unitSelectedCount++;
        foreach (var interactable in _interactables)
        {
            interactable.DisplayVisuals();
        }
    }

    private void HandleUnitDeselected()
    {
        _unitSelectedCount--;
        if (_unitSelectedCount <= 0)
        {
            foreach (var interactable in _interactables)
            {
                interactable.RemoveVisuals();
            }
        }
    }

    public void SetHoveredInteractable(InteractableController interactable)
    {
        AwaitInput();
        _hoveredInteractable = interactable;
    }

    public void RemoveHoveredInteractable(InteractableController interactable)
    {
        StopAwait();
        if (_hoveredInteractable == interactable) _hoveredInteractable = null;
    }

    private void AwaitInput()
    {
        InputManager.OnRightClick += Interact;
    }

    private void StopAwait()
    {
        InputManager.OnRightClick -= Interact;
    }

    private void Interact()
    {
        if (_hoveredInteractable == null) return;
        foreach (var unit in UnitManager.Instance.SelectedUnits)
        {
            unit.Interact(_hoveredInteractable);
        }
    }
}
