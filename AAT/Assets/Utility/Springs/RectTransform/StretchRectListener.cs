using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchRectListener : SpringListener
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField, ShowIf(nameof(useSetValue), true, 3)] 
    private RectStretchValues minValue, origValue, maxValue;
    
    private RectStretchValues _origValue;
    
    private void Start()
    {
        _origValue = useSetValue ? origValue : GetOrig();
        if (useSetValue) return;
        
        minValue = _origValue * minMultiplier;
        maxValue = _origValue * maxMultiplier;
    }

    private RectStretchValues GetOrig()
    {
        return new RectStretchValues(rectTransform.offsetMin, rectTransform.offsetMax);
    }

    public override void HandleSpringValue(float amount, float target)
    {
        switch (amount)
        {
            case > 0:
                ChangeValue(_origValue + (maxValue - _origValue) * amount);
                break;
            case < 0:
                ChangeValue(_origValue + (_origValue - minValue) * amount);
                break;
        }
    }

    private void ChangeValue(RectStretchValues value)
    {
        rectTransform.offsetMin = value.LeftBottom;
        rectTransform.offsetMax = value.RightTop;
    }
}