using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Data/AOE Knockback Component")]
public class ProjectileKnockbackComponentData : ProjectileComponentData
{
    [SerializeField] private float knockbackRadius;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackLerpEndPercent;

    public override void ActivateComponent(ProjectileController from, GameObject _, float damage)
    {
        damage = -Mathf.Abs(damage);
        var enemyLayer = TeamManager.Instance.GetEnemyLayer(from.Team.GetTeamNumber());
        List<LagCompensatedHit> hits = new();
        from.Runner.LagCompensation.OverlapSphere(from.transform.position, knockbackRadius, from.Object.InputAuthority, hits, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.GameObject == null) continue;
            
            var enemyTransform = hit.GameObject.transform;
            var direction = enemyTransform.position - from.transform.position;
            KnockbackManager.Instance.AddKnockback(enemyTransform, direction, knockbackDistance, knockbackSpeed, knockbackLerpEndPercent);
        }
    }
}
