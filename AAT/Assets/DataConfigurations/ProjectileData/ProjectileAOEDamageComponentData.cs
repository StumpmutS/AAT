using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Data/AOE Damage Component")]
public class ProjectileAOEDamageComponentData : ProjectileComponentData
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] private float damageRadius;

    public override void ActivateComponent(ProjectileController from, GameObject _, float damage)
    {
        damage = -Mathf.Abs(damage);
        var enemyLayer = TeamManager.Instance.GetEnemyLayer(from.Team.GetTeamNumber());
        List<LagCompensatedHit> hits = new();
        from.Runner.LagCompensation.OverlapSphere(from.transform.position, damageRadius, from.Object.InputAuthority, hits, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.GameObject != null) hit.GameObject.GetComponent<IHealth>().ModifyHealth(damage, null, new AttackDecalInfo());
        }
    }
}
