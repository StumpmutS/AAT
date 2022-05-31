using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/In Sight Range")]
public class InSightRangeTransition : Transition
{
    public override bool Decision(UnitController unit)
    {
        return Physics.CheckSphere(unit.transform.position,
            unit.Stats.CurrentStats[EUnitFloatStats.SightRange], unit.EnemyLayer);
    }
}
