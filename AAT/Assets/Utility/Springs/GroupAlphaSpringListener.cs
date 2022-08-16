using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupAlphaSpringListener : FloatSpringListener
{
    [SerializeField] private ERangeContainment rangeContainType;
    [SerializeField] private List<Image> images;

    protected override float GetOrig()
    {
        return 1;
    }

    protected override void ChangeValue(float value)
    {
        foreach (var image in images)
        {
            var color = image.color;
            color.a = rangeContainType == ERangeContainment.ClampZero ? ClampZero(value) : Mathf.Abs(value);
            image.color = color;
        }
    }

    private float ClampZero(float value)
    {
        if (value < 0) value = 0;
        return value;
    }
}

public enum ERangeContainment
{
    None,
    AbsoluteValue,
    ClampZero
}