using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAttack : MonoBehaviour
{
    [SerializeField] private GameObject iAttackSystem;

    private IAttackSystem _attackSystem;

    private void Start()
    {
        if (iAttackSystem.TryGetComponent<IAttackSystem>(out var system))
        {
            _attackSystem = system;
        }
    }

    private void Attack()
    {
        _attackSystem.CallAnimationAttack();
    }
}
