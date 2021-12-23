using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Dash Ability Component")]
public class DashAbilityComponentData : AbilityComponent
{
    [SerializeField] private Vector3 DashDistance;
    [SerializeField][Min(100f)] private float DashSpeedPercentMultiplier;

    private Dictionary<UnitController, float> _unitsDashed = new Dictionary<UnitController, float>();

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        Dash(unit);
    }

    private void Dash(UnitController unit)
    {
        if (!_unitsDashed.ContainsKey(unit))
        {
            _unitsDashed[unit] = 0;
            float speedAdded = DashSpeedPercentMultiplier / 100 * unit.UnitStatsModifierManager.CurrentUnitStatsData[EUnitFloatStats.MovementSpeed] 
                                                              - unit.UnitStatsModifierManager.CurrentUnitStatsData[EUnitFloatStats.MovementSpeed];
            unit.UnitStatsModifierManager.ModifyFloatStat(EUnitFloatStats.MovementSpeed, speedAdded);                                                                      
            _unitsDashed[unit] = speedAdded;
        }
        
        var targetPosition = unit.transform.position +
            (unit.transform.right * DashDistance.x) +
            (unit.transform.up * DashDistance.y) +
            (unit.transform.forward * DashDistance.z);


        unit.transform.position = Vector3.MoveTowards(unit.transform.position, targetPosition, 
            unit.UnitStatsModifierManager.CurrentUnitStatsData[EUnitFloatStats.MovementSpeed] * Time.deltaTime);
    }

    public override void DeactivateComponent(UnitController unit)
    {
        unit.UnitStatsModifierManager.ModifyFloatStat(EUnitFloatStats.MovementSpeed, -_unitsDashed[unit]);
        _unitsDashed.Remove(unit);
    }
}