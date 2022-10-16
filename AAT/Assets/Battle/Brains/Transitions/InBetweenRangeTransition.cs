using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/In Between Range")]
public class InBetweenRangeTransition : Transition<AiTransitionBlackboard>
{
    public override bool Decision(AiTransitionBlackboard transitionBlackboard)
    {
        return !transitionBlackboard.InAttackRange && transitionBlackboard.InSightRange;
    }
}