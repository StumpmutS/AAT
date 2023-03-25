using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Has Click Target")]
public class HasRightClickTargetTransition : AiTransition
{
    public override bool Decision(AiTransitionBlackboard transitionBlackboard)
    {
        return transitionBlackboard.Selected && Player.RightClickTarget != null;
    }
}