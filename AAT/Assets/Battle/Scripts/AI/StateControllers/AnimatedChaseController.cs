using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedChaseController : BaseChaseController
{
    [SerializeField] private UnitAnimationController unitAnimation;
    
    protected override void Chase(Collider target)
    {
        base.Chase(target);
        unitAnimation.SetMovement(agent.Speed);
    }
}
