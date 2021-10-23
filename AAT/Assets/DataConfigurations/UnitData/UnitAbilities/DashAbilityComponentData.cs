using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Dash Ability Component")]
public class DashAbilityComponentData : AbilityComponent
{
    [SerializeField] private Vector3 DashDistance;
    [SerializeField] private float DashSpeed;

    public override void ActivateComponent(UnitController unit)
    {
        Dash(unit);
    }

    private void Dash(UnitController unit)
    {
        var targetPosition = unit.transform.position +
            (unit.transform.right * DashDistance.x) +
            (unit.transform.up * DashDistance.y) +
            (unit.transform.forward * DashDistance.z);
        unit.transform.position = Vector3.MoveTowards(unit.transform.position, targetPosition, DashSpeed * Time.deltaTime);
    }
}