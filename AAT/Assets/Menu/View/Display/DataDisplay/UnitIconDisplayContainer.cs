using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitIconDisplayContainer : InteractableDataContainer
{
    [SerializeField] private LayoutDisplay layoutDisplay;
    [SerializeField] private PropertyHandler unitIconPrefab;
    
    public override void DisplayData(List<StumpData> data, UnityAction<int, StumpData> callback)
    {
        layoutDisplay.Clear();
        var unitData = data.Select(d => (UnitData) d).ToList();
        
        for (int i = 0; i < unitData.Count; i++)
        {
            var property = Instantiate(unitIconPrefab); 
            property.Init(layoutDisplay.gameObject, callback, unitData[i]);
            var image = property.Image;
            if (image != null)
            {
                image.sprite = unitData[i].UnitArtData.UnitIcon;
                image.color = Color.white;
            }
            layoutDisplay.Add(property.transform);

            if (i == 0) property.GetComponent<Toggle>().isOn = true;
        }
    }

    public override void RemoveDisplay()
    {
        layoutDisplay.Clear();
    }
}