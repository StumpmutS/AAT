using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GroupAlphaSpringListener : FloatSpringListener, ISerializationCallbackReceiver
{
    [SerializeField] private ERangeContainment rangeContainType;
    [SerializeField] private List<Image> images;

    protected override float GetOrig()
    {
        return origValue;
    }

    protected override void ChangeValue(float value, float _)
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

    public void OnBeforeSerialize()
    {
        useSetValue = true;
    }

    public void OnAfterDeserialize()
    {
        useSetValue = true;
    }
}