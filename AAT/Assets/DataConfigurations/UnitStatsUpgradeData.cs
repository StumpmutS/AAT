using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Upgrade Data")]
public class UnitStatsUpgradeData : ScriptableObject
{
    public List<EStat> StatsToModify;
    public List<float> AmountsToModifyBy;
    public ETransportationType TransportationTypeToModify;
    public EAttackType AttackTypeToModify;
    public EMovementType MovementTypeToModify;
}
