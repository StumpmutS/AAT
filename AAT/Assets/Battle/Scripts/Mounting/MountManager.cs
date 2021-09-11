using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountManager : MonoBehaviour
{
    private static MountManager instance;
    public static MountManager Instance => instance;

    private HashSet<MountableController> _mountableControllers = new HashSet<MountableController>();
    private HashSet<TransportableController> _transportableControllers = new HashSet<TransportableController>();
    private List<TransportableController> _selectedTransportables = new List<TransportableController>();
    private UnitController _selectedUnitType;

    private MountableController _hoveredMountable;

    private void Awake()
    {
        instance = this;
    }

    public void AddMountable(MountableController mountableController)
    {
        _mountableControllers.Add(mountableController);
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
            mountable.OnMountableHoverStop += RemoveMountablePreview;
        }
    }

    private void RemoveMountableVisuals(TransportableController none)
    {
        foreach (var mountable in _mountableControllers)
        {
            mountable.RemoveVisuals();
            mountable.OnMountableHover -= DisplayMountablePreview;
            mountable.OnMountableHoverStop -= RemoveMountablePreview;
        }
    }

    private void DisplayMountablePreview(MountableController mountableController)
    {
        foreach (var mountable in _mountableControllers)
        {
            mountable.RemoveVisuals();                
        }
        mountableController.DisplayPreview(_selectedUnitType.UnitVisuals, _selectedTransportables.Count);
        _hoveredMountable = mountableController;
        InputManager.OnRightClick += TransportablePerformMount;
    }

    private void TransportablePerformMount()
    {
        for (int i = 0; i < _selectedTransportables.Count; i++)
        {
            _selectedTransportables[i].BeginMountProcess(_hoveredMountable, i);
        }
    }

    private void RemoveMountablePreview(MountableController mountableController)
    {
        mountableController.RemovePreview();
        InputManager.OnRightClick -= TransportablePerformMount;
    }
}
