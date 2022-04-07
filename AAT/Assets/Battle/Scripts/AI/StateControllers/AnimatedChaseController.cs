using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedChaseController : BaseChaseController
{
    [SerializeField] private UnitAnimationController unitAnimation;
    [SerializeField] private bool animateChase;
    
    protected override void Chase(Collider target)
    {
        base.Chase(target);
        unitAnimation.SetMovement(agent.Speed);
        unitAnimation.SetChase(true);
    }

    protected override void StopChase()
    {
        if (animateChase)
            unitAnimation.SetChase(false);
    }
}
