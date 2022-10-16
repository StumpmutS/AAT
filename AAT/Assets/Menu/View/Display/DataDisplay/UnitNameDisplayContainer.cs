using System;
using TMPro;
using UnityEngine;

public class UnitNameDisplayContainer : DataDisplayContainer
{
    [SerializeField] private TMP_Text text;

    public override void DisplayData(object data)
    {
        if (data == null)
        {
            text.text = string.Empty;
            return;
        }
        
        var unitData = (UnitData) data;
        text.text = unitData.GeneralUnitData.UnitName;
    }
}
