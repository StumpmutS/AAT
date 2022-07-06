using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/In Between Range")]
public class InBetweenRangeTransition : Transition
{
    public override bool Decision(UnitController unit)
    {
        return !Physics.CheckSphere(unit.transform.position, unit.Stats.GetStat(EUnitFloatStats.AttackRange), unit.EnemyLayer) 
               && Physics.CheckSphere(unit.transform.position, unit.Stats.GetStat(EUnitFloatStats.SightRange), unit.EnemyLayer);
    }
}