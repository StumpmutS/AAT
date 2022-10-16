using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/In Attack Range")]
public class InAttackRangeTransition : Transition<AiTransitionBlackboard>
{
    public override bool Decision(AiTransitionBlackboard transitionBlackboard)
    {
        return transitionBlackboard.InAttackRange;
    }
}
