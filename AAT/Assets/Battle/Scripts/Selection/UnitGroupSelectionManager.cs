using System.Collections.Generic;
using UnityEngine;

public class UnitGroupSelectionManager : MonoBehaviour
{
    [SerializeField] private List<UnitDeterminedUI> unitGroupUI;

    //private bool _unitGroupUIEnabled = false;
    private HashSet<UnitGroupController> _selectedUnitGroups = new();
    public static UnitGroupSelectionManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AddUnitGroup(UnitGroupController unitGroup)
    {
        /*_selectedUnitGroups.Add(unitGroup);
        if (_unitGroupUIEnabled) return;
        foreach (var UIElement in unitGroupUI)
        {
            UIElement.gameObject.SetActive(true);
            UIElement.SetToUnitPreference(unitGroup.GetChaseStates());
        }*/
    }

    public void RemoveUnitGroup(UnitGroupController unitGroup)
    {
        /*_selectedUnitGroups.Remove(unitGroup);
        if (_selectedUnitGroups.Count != 0) return;
        foreach (var UIelement in unitGroupUI)
        {
            UIelement.gameObject.SetActive(false);
        }*/
    }

    public void SetChaseStates(bool value)
    {
        /*foreach (var unitGroup in _selectedUnitGroups)
        {
            unitGroup.SetChaseStates(value);
        }*/
    }
}
