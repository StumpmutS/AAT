using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountManager : MonoBehaviour
{
    private static MountManager instance;
    public static MountManager Instance => instance;

    private HashSet<BaseMountableController> _mountableControllers = new HashSet<BaseMountableController>();
    private HashSet<TransportableController> _transportableControllers = new HashSet<TransportableController>();
    private List<TransportableController> _selectedTransportables = new List<TransportableController>();
    private UnitController _selectedUnitType;

    private List<BaseMountableController> _hoveredMountables = new List<BaseMountableController>();

    private void Awake()
    {
        instance = this;
    }

    public void AddMountable(BaseMountableController mountable)
    {
        _mountableControllers.Add(mountable);
    }

    public void AddHoveredMountable(BaseMountableController mountable)
    {
        _hoveredMountables.Add(mountable);
    }

    public void AddTransportable(TransportableController transportableController)
    {
        _transportableControllers.Add(transportableController);
        transportableController.OnTransportableSelect += DisplayMountableVisuals;
        transportableController.OnTransportableSelect += AddSelectedTransportable;
        transportableController.OnTransportableDeselect += RemoveMountableVisuals;
        transportableController.OnTransportableDeselect += RemoveSelectedTransportable;
    }

    private void AddSelectedTransportable(TransportableController transportable)
    {
        _selectedTransportables.Add(transportable);
    }

    private void RemoveSelectedTransportable(TransportableController transportable)
    {
        _selectedTransportables.Remove(transportable);
    }

    private void DisplayMountableVisuals(TransportableController transportableController)
    {
        _selectedUnitType = transportableController.UnitController;
        foreach (var mountable in _mountableControllers)
        {
            mountable.DisplayVisuals();
            mountable.OnMountableHover += DisplayMountablePreview;
            mountable.OnMountableHoverStop += RemoveMountablePreviews;
        }
    }

    private void RemoveMountableVisuals(TransportableController none)
    {
        foreach (var mountable in _mountableControllers)
        {
            mountable.RemoveVisuals();
            mountable.OnMountableHover -= DisplayMountablePreview;
            mountable.OnMountableHoverStop -= RemoveMountablePreviews;
        }
    }

    private void DisplayMountablePreview(BaseMountableController mountable)
    {
        foreach (var mountableController in _mountableControllers)
        {
            mountableController.RemoveVisuals();                
        }
        mountable.CallDisplayPreview(_selectedUnitType.UnitVisuals, _selectedTransportables.Count);
        InputManager.OnRightClick += TransportablePerformMount;
    }

    private void TransportablePerformMount()
    {
        for (int i = 0; i < _hoveredMountables.Count; i++)
        {
            _selectedTransportables[i].BeginMountProcess(_hoveredMountables[i]);
        }
    }

    private void RemoveMountablePreviews()
    {
        foreach (var mountable in _hoveredMountables)
        {
            mountable.RemovePreview();
        }
        _hoveredMountables.Clear();
        foreach (var mountable in _mountableControllers)
        {
            mountable.DisplayVisuals();
            mountable.OnMountableHover += DisplayMountablePreview;
            mountable.OnMountableHoverStop += RemoveMountablePreviews;
        }
        InputManager.OnRightClick -= TransportablePerformMount;
    }
}
