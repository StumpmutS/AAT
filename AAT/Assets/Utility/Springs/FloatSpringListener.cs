public abstract class FloatSpringListener : SpringListener
{
    private float _minValue;
    private float _maxValue;
    
    private float _origValue;
    
    private void Start()
    {
        _origValue = GetOrig();
        _minValue = _origValue * minMultiplier;
        _maxValue = _origValue * maxMultiplier;
    }

    protected abstract float GetOrig();

    public override void HandleSpringValue(float amount, float target)
    {
        switch (amount)
        {
            case > 0:
                ChangeValue(_origValue + (_maxValue - _origValue) * amount);
                break;
            case < 0:
                ChangeValue(_origValue + (_origValue - _minValue) * amount);
                break;
        }
    }

    protected abstract void ChangeValue(float value);
}
