using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager Instance { get; private set; }

    private HashSet<InteractableController> _interactables = new();
    private int _unitSelectedCount;
    private PoolingObject _selectedUnitPreview;
    private InteractableController _hoveredInteractable;
    private Dictionary<UnitController, MovementInteractOverrideComponentState> _interactionStates = new();

    private void Awake() => Instance = this;

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

        if (!_interactionStates.ContainsKey(unit))
        {
            if (unit.TryGetComponent<MovementInteractOverrideComponentState>(out var state))
            {
                _interactionStates[unit] = state;
            }
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

        _interactionStates.Remove(unit);
    }

    public InteractableController GetHoveredInteractable(UnitController unit)
    {
        return _hoveredInteractable == null ? null : _hoveredInteractable.DetermineInteractable(unit);
    }
    
    public void SetHoveredInteractable(InteractableController interactable)
    {
        _hoveredInteractable = interactable;
        if (_unitSelectedCount <= 0) return;
        foreach (var Interactable in _interactables)
        {
            Interactable.RemovePreview();
        }
        if (_selectedUnitPreview == null) return;
        interactable.CallDisplayPreview(_selectedUnitPreview, _unitSelectedCount);//TODO: CALL MOUNTABLE SETUP FORM HERE TOO, MOVE COUNT OF SELECTED UNITS TO PUBLIC STATIC IN UNITMANAGER
    }

    public void RemoveHoveredInteractable(InteractableController interactable)
    {
        if (_hoveredInteractable == interactable) _hoveredInteractable = null;
        if (_unitSelectedCount <= 0) return;
        foreach (var Interactable in _interactables)
        {
            Interactable.RemovePreview();
        }
    }
}
