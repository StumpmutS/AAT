using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/In Between Range")]
public class InBetweenRangeTransition : Transition
{
    public override bool Decision(UnitController unit)
    {
        Debug.LogError("DONT USE"); //todo
        return false;

        /*return unit.Runner.LagCompensation.OverlapSphere(unit.transform.position,
                   unit.Stats.GetStat(EUnitFloatStats.AttackRange), unit.Object.InputAuthority, 
                   new List<LagCompensatedHit>(), TeamManager.Instance.GetEnemyLayer(unit.Team.GetTeamNumber())) < 1
               && unit.Runner.LagCompensation.OverlapSphere(unit.transform.position,
                   unit.Stats.GetStat(EUnitFloatStats.SightRange), unit.Object.InputAuthority, 
                   new List<LagCompensatedHit>(), TeamManager.Instance.GetEnemyLayer(unit.Team.GetTeamNumber())) > 0;*/
    }
}