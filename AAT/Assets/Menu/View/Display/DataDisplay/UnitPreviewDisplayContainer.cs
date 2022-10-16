using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPreviewDisplayContainer : DataDisplayContainer
{
    private GameObject _currentPreview;
    
    public override void DisplayData(object data)
    {
        if (_currentPreview != null) Destroy(_currentPreview);
        if (data == null) return;
        
        var unitData = (UnitData) data;
        _currentPreview = Instantiate(unitData.UnitArtData.UnitPreview, transform);
    }
}
