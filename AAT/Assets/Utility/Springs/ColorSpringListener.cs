using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColorSpringListener : SpringListener
{
    [SerializeField, ShowIf(nameof(useSetValue), true)] private Color minValue, origValue, maxValue;
    
    private Color _origValue;
    
    private void Start()
    {
        _origValue = useSetValue ? origValue : GetOrig();
        if (useSetValue) return;

        minValue = _origValue * minMultiplier;
        maxValue = _origValue * maxMultiplier;
    }

    protected abstract Color GetOrig();

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

    protected abstract void ChangeValue(Color value);

    public void SetMaxColor(Color color)
    {
        maxValue = color;
    }
}
