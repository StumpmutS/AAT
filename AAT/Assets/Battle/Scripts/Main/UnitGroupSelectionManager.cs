using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroupSelectionManager : MonoBehaviour
{
    [SerializeField] private List<UnitDeterminedUI> unitGroupUI;

    private bool unitGroupUIEnabled = false;
    private HashSet<UnitGroupController> selectedUnitGroups = new HashSet<UnitGroupController>();
    private static UnitGroupSelectionManager instance;
    public static UnitGroupSelectionManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddUnitGroup(UnitGroupController unitGroup)
    {
        selectedUnitGroups.Add(unitGroup);
        if (!unitGroupUIEnabled) foreach (var UIElement in unitGroupUI)
        {
            UIElement.gameObject.SetActive(true);
            UIElement.SetToUnitPreference(unitGroup.GetChaseStates());
        }
    }

    public void RemoveUnitGroup(UnitGroupController unitGroup)
    {
        selectedUnitGroups.Remove(unitGroup);
        if (selectedUnitGroups.Count == 0) foreach (var UIelement in unitGroupUI)
        {
            UIelement.gameObject.SetActive(false);
        }
    }

    public void SetChaseStates(bool value)
    {
        foreach (var unitGroup in selectedUnitGroups)
        {
            unitGroup.SetChaseStates(value);
        }
    }
}
