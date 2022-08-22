using FMPUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class SpringController : MonoBehaviour
{
    [SerializeField] private float frequency;
    [SerializeField] private float damping;

    private float _targetValue;
    private float _currentValue;
    private float _currentVelocity;
    private bool _pressed;

    public UnityEvent<float, float> OnSpringValueChanged;
    
    public void Update()
    {
        SpringMotion.CalcDampedSimpleHarmonicMotion(ref _currentValue, ref _currentVelocity, 
            _targetValue, Time.deltaTime, frequency, damping);
        OnSpringValueChanged.Invoke(_currentValue, _targetValue);
    }

    public virtual void Nudge(float amount)
    {
        _currentVelocity += amount;
    }

    public void SetTarget(float value)
    {
        _targetValue = Mathf.Clamp(value, -1, 1);
    }
}
