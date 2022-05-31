using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Data/AOE Knockback Component")]
public class ProjectileKnockbackComponentData : ProjectileComponentData
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float knockbackRadius;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackLerpEndPercent;

    public override void ActivateComponent(GameObject from, GameObject hit, float damage)
    {
        Collider[] enemyCollidersHit = new Collider[25];
        Physics.OverlapSphereNonAlloc(from.transform.position, knockbackRadius, enemyCollidersHit, enemyLayer);
        foreach (var enemyCollider in enemyCollidersHit)
        {
            if (enemyCollider == null) continue;
            Transform enemyTransform = enemyCollider.transform;
            var direction = (enemyTransform.position - hit.transform.position);
            KnockbackManager.Instance.AddKnockback(enemyTransform, direction, knockbackDistance, knockbackSpeed, knockbackLerpEndPercent);
        }
    }
}
