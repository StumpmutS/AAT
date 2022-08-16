using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/In Sight Range")]
public class InSightRangeTransition : Transition
{
    public override bool Decision(UnitController unit)
    {
        return unit.GetComponent<TargetFinder>().SightTarget != null;
    }
}
