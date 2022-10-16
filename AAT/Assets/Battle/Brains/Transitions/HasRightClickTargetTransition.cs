using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Has Click Target")]
public class HasRightClickTargetTransition : Transition<TransitionBlackboard>
{
    public override bool Decision(TransitionBlackboard transitionBlackboard)
    {
        if (!transitionBlackboard.Selected) return false;
        var hit = Player.RightClickTarget;
        return hit.Hit != null;
    }
}