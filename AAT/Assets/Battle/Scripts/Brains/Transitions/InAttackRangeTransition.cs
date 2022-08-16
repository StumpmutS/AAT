using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/In Attack Range")]
public class InAttackRangeTransition : Transition
{
    public override bool Decision(UnitController unit)
    {
        return unit.GetComponent<TargetFinder>().AttackTarget != null;
    }
}
