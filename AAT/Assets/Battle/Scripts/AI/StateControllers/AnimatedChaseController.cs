using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedChaseController : BaseChaseController
{
    [SerializeField] protected UnitAnimationController unitAnimation;
    
    protected override void Chase(GameObject target)
    {
        base.Chase(target);
        unitAnimation.SetMovement(agent.speed);
    }
}