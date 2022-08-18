using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCrit : MonoBehaviour
{
    [SerializeField] private Brain brain;

    private AttackComponentState _attackState;

    private void Start()
    {
        if (brain.Container.TryGetComponentState(typeof(AttackComponentState), out var state))
        {
            _attackState = (AttackComponentState) state;
        }
    }

    private void Crit()
    {
        _attackState.AnimationTriggeredCrit();
    }
}
