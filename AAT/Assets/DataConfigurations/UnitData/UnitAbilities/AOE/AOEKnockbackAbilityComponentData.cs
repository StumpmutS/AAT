using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/AOE/Knockback Ability Component")]
public class AOEKnockbackAbilityComponentData : AbilityComponent
{
    [SerializeField] private float knockbackRadius;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackLerpEndPercent;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        var enemyLayer = TeamManager.Instance.GetEnemyLayer(unit.Team.GetTeamNumber());
        List<LagCompensatedHit> enemyCollidersHit = new();
        unit.Runner.LagCompensation.OverlapSphere(unit.transform.position, knockbackRadius, unit.Object.InputAuthority, enemyCollidersHit, enemyLayer);
        foreach (var hit in enemyCollidersHit)
        {
            if (hit.GameObject == null) continue;
            Transform enemyTransform = hit.GameObject.transform;
            var direction = (enemyTransform.position - unit.transform.position);
            KnockbackManager.Instance.AddKnockback(enemyTransform, direction, knockbackDistance, knockbackSpeed, knockbackLerpEndPercent);
        }
    }
}
