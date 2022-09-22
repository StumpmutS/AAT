using UnityEngine;

public abstract class FloatSpringListener : SpringListener
{
    [SerializeField, ShowIf(nameof(useSetValue), true)] protected float minValue, origValue, maxValue;

    private float _origValue;
    
    private void Start()
    {
        _origValue = GetOrig();
        if (useSetValue) return;
        minValue = _origValue * minMultiplier;
        maxValue = _origValue * maxMultiplier;
    }

    protected abstract float GetOrig();

    public override void HandleSpringValue(float amount, float target)
    {
        switch (amount)
        {
            case > 0:
                ChangeValue(_origValue + (maxValue - _origValue) * amount, target);
                break;
            case < 0:
                ChangeValue(_origValue + (_origValue - minValue) * amount, target);
                break;
        }
    }

    protected abstract void ChangeValue(float value, float target);
}
