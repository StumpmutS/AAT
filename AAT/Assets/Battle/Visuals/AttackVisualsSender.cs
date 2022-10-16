using System.Collections.Generic;
using UnityEngine;

public class AttackVisualsSender : VisualsSender
{
    [SerializeField] private NetworkComponentStateContainer<AiTransitionBlackboard> networkContainer;
    [SerializeField] private Color attackColor = Color.white;
    [SerializeField] private Color critColor = Color.red;

    private AttackComponentState _attackState;

    private void Start()
    {
        if (networkContainer.TryGetComponentState(typeof(AttackComponentState), out var state))
        {
            _attackState = (AttackComponentState) state;
            _attackState.OnAttack += HandleAttack;
            _attackState.OnCrit += HandleCrit;
        }
    }

    private void HandleAttack(GameObject target, float amount)
    {
        SetupData(target, amount, attackColor);
    }

    private void HandleCrit(GameObject target, float amount)
    {
        SetupData(target, amount, critColor);
    }

    private void SetupData(GameObject target, float amount, Color color)
    {
        Dictionary<VisualComponent, VisualInfo> newVisuals = new();

        if (target.TryGetComponent<Health>(out var health)) 
        {
            foreach (var kvp in visuals)
            {
                var severity = kvp.Value.Severity;
                health.CalculateDamageSeverity(ref amount, ref severity);
                newVisuals[kvp.Key] = OverrideValues(kvp.Value, severity, color);
            }
        }
        
        SendVisuals(target, newVisuals);
    }

    private VisualInfo OverrideValues(VisualInfo info, int severity, Color color)
    {
        info.Severity = severity;
        info.Color = color;
        return info;
    }

    private void OnDestroy()
    {
        if (_attackState != null) _attackState.OnAttack -= HandleAttack;
        if (_attackState != null) _attackState.OnCrit -= HandleCrit;
    }
}