using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitIconDisplayContainer : InteractableDataContainer
{
    [SerializeField] private LayoutDisplay layoutDisplay;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private StylizedImageToggleProperty unitIconPrefab;
    
    public override void DisplayData(List<StumpData> data, Action<object> callback)
    {
        layoutDisplay.Clear();
        var unitData = data.Select(d => (UnitData) d).ToList();
        
        for (int i = 0; i < unitData.Count; i++)
        {
            var property = Instantiate(unitIconPrefab);
            property.Init(unitData[i].UnitArtData.UnitIcon, callback, unitData[i]);
            
            property.Toggle.group = toggleGroup;
            if (i == 0) property.Toggle.isOn = true;
            layoutDisplay.Add(property.transform);
        }
    }

    public override void RemoveDisplay()
    {
        layoutDisplay.Clear();
    }
}