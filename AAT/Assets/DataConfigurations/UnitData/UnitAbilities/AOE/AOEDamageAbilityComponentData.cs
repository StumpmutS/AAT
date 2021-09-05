using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/AOE/Damage Ability Component")]
public class AOEDamageAbilityComponentData : AbilityComponent
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] private float damageRadius;
    [SerializeField] private float damage;

    private Dictionary<GameObject, List<Collider>> damagedEnemies = new Dictionary<GameObject, List<Collider>>();

    public override void ActivateComponent(GameObject gameObject)
    {
        damage = -Mathf.Abs(damage);
        Collider[] enemyCollidersHit = new Collider[25];
        Physics.OverlapSphereNonAlloc(gameObject.transform.position, damageRadius, enemyCollidersHit, enemyLayer);
        foreach (var enemyCollider in enemyCollidersHit)
        {
            if (enemyCollider == null) continue;
            if (damagedEnemies.ContainsKey(gameObject))
            {
                if (damagedEnemies[gameObject].Contains(enemyCollider)) continue;
            }
            else damagedEnemies[gameObject] = new List<Collider>();

            enemyCollider.GetComponent<IHealth>().ModifyHealth(damage);
            damagedEnemies[gameObject].Add(enemyCollider);
        }
    }

    public override void DeactivateComponent(GameObject gameObject)
    {
        if (damagedEnemies.ContainsKey(gameObject))
            damagedEnemies[gameObject].Clear();
    }
}
