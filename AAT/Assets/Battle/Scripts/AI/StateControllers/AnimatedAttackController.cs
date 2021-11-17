using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitAnimationController))]
public class AnimatedAttackController : BaseAttackController
{
    protected UnitAnimationController unitAnimation;

    protected override void Awake()
    {
        base.Awake();
        unitAnimation = GetComponent<UnitAnimationController>();
    }

    protected override void BaseAttack(UnitController target)
    {
        unitAnimation.SetAttack(true);
        base.BaseAttack(target);
    }

    protected override void CritAttack(UnitController target)
    {
        unitAnimation.SetCrit(true);
        base.CritAttack(target);
    }
}
