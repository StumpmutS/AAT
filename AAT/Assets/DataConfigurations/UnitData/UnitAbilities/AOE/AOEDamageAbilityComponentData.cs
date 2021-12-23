using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/AOE/Damage Ability Component")]
public class AOEDamageAbilityComponentData : AbilityComponent
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] private float damageRadius;
    [SerializeField] private float damage;

    private Dictionary<UnitController, List<Collider>> damagedEnemies = new Dictionary<UnitController, List<Collider>>();

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        damage = -Mathf.Abs(damage);
        Collider[] enemyCollidersHit = new Collider[25];
        Physics.OverlapSphereNonAlloc(unit.transform.position, damageRadius, enemyCollidersHit, enemyLayer);
        foreach (var enemyCollider in enemyCollidersHit)
        {
            if (enemyCollider == null) continue;
            if (damagedEnemies.ContainsKey(unit))
            {
                if (damagedEnemies[unit].Contains(enemyCollider)) continue;
            }
            else damagedEnemies[unit] = new List<Collider>();

            enemyCollider.GetComponent<IHealth>().ModifyHealth(damage);
            damagedEnemies[unit].Add(enemyCollider);
        }
    }

    public override void DeactivateComponent(UnitController unit)
    {
        if (damagedEnemies.ContainsKey(unit))
            damagedEnemies[unit].Clear();
    }
}
