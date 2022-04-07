using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager Instance { get; private set; }

    private HashSet<InteractableController> _interactables = new HashSet<InteractableController>();
    private int _unitSelectedCount;
    private InteractableController _hoveredInteractable;
    private PoolingObject _selectedUnitPreview;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UnitManager.Instance.OnUnitSelected += HandleUnitSelected;
        UnitManager.Instance.OnUnitDeselected += HandleUnitDeselected;
    }

    public void AddInteractable(InteractableController interactable)
    {
        _interactables.Add(interactable);
    }

    private void HandleUnitSelected(UnitController unit)
    {
        _unitSelectedCount++;
        foreach (var interactable in _interactables.Where(interactable => unit.InteractableTypes.Contains(interactable.InteractableType)))
        {
            interactable.DisplayVisuals();
        }

        _selectedUnitPreview = unit.UnitVisuals;
    }

    private void HandleUnitDeselected(UnitController unit)
    {
        _unitSelectedCount--;
        if (_unitSelectedCount > 0) return;
        foreach (var interactable in _interactables)
        {
            interactable.RemoveVisuals();
            interactable.RemovePreview();
        }
    }

    public void SetHoveredInteractable(InteractableController interactable)
    {
        _hoveredInteractable = interactable;
        if (_unitSelectedCount <= 0) return;
        AwaitInput();
        foreach (var Interactable in _interactables)
        {
            Interactable.RemovePreview();
        }
        if (_selectedUnitPreview == null) return;
        interactable.CallDisplayPreview(_selectedUnitPreview, _unitSelectedCount);
    }

    public void RemoveHoveredInteractable(InteractableController interactable)
    {
        if (_hoveredInteractable == interactable) _hoveredInteractable = null;
        if (_unitSelectedCount <= 0) return;
        StopAwait();
        foreach (var Interactable in _interactables)
        {
            Interactable.RemovePreview();
        }
    }

    private void AwaitInput()
    {
        InputManager.OnRightClickDown += Interact;
    }

    private void StopAwait()
    {
        InputManager.OnRightClickDown -= Interact;
    }

    private void Interact()
    {
        if (_hoveredInteractable == null) return;
        _hoveredInteractable.SetupInteractions(UnitManager.Instance.SelectedUnits.ToList());
    }
}
