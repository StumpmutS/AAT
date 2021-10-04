using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/AOE/Knockback Ability Component")]
public class AOEKnockbackAbilityComponentData : AbilityComponent
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float knockbackRadius;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackLerpEndPercent;

    public override void ActivateComponent(GameObject gameObject)
    {
        Collider[] enemyCollidersHit = new Collider[25];
        Physics.OverlapSphereNonAlloc(gameObject.transform.position, knockbackRadius, enemyCollidersHit, enemyLayer);
        foreach (var enemyCollider in enemyCollidersHit)
        {
            if (enemyCollider == null) continue;
            Transform enemyTransform = enemyCollider.transform;
            var direction = (enemyTransform.position - gameObject.transform.position);
            KnockbackManager.Instance.AddKnockback(enemyTransform, direction, knockbackDistance, knockbackSpeed, knockbackLerpEndPercent);
        }
    }
}