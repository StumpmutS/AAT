using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Data/AOE Damage Component")]
public class ProjectileAOEDamageComponentData : ProjectileComponentData
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] private float damageRadius;

    public override void ActivateComponent(GameObject from, GameObject hit, float damage)
    {
        damage = -Mathf.Abs(damage);
        Collider[] enemyCollidersHit = new Collider[25];
        Physics.OverlapSphereNonAlloc(from.transform.position, damageRadius, enemyCollidersHit, enemyLayer);
        foreach (var enemyCollider in enemyCollidersHit)
        {
            if (enemyCollider != null) enemyCollider.GetComponent<IHealth>().ModifyHealth(damage);
        }
    }
}
