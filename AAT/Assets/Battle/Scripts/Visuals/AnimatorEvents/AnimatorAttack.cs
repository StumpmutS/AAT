using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAttack : MonoBehaviour
{
    [SerializeField] private NetworkStateComponentContainer container;

    private AttackComponentState _attackState;

    private void Start()
    {
        if (container.TryGetComponentState(typeof(AttackComponentState), out var state))
        {
            _attackState = (AttackComponentState) state;
        }
    }

    private void Attack()
    {
        _attackState.AnimationTriggeredAttack();
    }
}
