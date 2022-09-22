using System;
using System.Collections;
using FMPUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class SpringController : MonoBehaviour
{
    [SerializeField] private float setTargetOnStart;
    [SerializeField] private float frequency;
    [SerializeField] private float damping;

    private float _targetValue;
    private float _currentValue;
    private float _currentVelocity;

    public UnityEvent<float, float> OnSpringValueChanged;

    private void OnEnable()
    {
        Reset();
        StartCoroutine(CoOnEnable());
    }

    private IEnumerator CoOnEnable()
    {
        yield return 0;
        _targetValue = setTargetOnStart;
    }

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

    [ContextMenu("Gather SpringListeners")]
    private void GatherSpringListeners()
    {
        for (int i = 0; i < OnSpringValueChanged.GetPersistentEventCount(); i++)
        {
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(OnSpringValueChanged, i);
        }
        
        foreach (var springListener in GetComponents<SpringListener>())
        {
            UnityEditor.Events.UnityEventTools.AddPersistentListener(OnSpringValueChanged, springListener.HandleSpringValue);
        }
    }

    public void Reset()
    {
        _targetValue = 0;
        _currentValue = 0;
        _currentVelocity = 0;
    }
}
