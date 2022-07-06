using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/In Attack Range")]
public class InAttackRangeTransition : Transition
{
    public override bool Decision(UnitController unit)
    {
        return Physics.CheckSphere(unit.transform.position,
            unit.Stats.GetStat(EUnitFloatStats.AttackRange), unit.EnemyLayer);
    }
}
