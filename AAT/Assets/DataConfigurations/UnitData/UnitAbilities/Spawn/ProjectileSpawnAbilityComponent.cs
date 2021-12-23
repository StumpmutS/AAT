using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Spawn/Projectile Spawn Ability Component")]
public class ProjectileSpawnAbilityComponent : AbilityComponent
{
    [SerializeField] private ProjectileController unitPrefab;
    [SerializeField] private int projectilesPerOffset;
    [SerializeField] private List<Vector3> spawnOffsets;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        var objTransform = unit.transform;
        
        for (int i = 0; i < spawnOffsets.Count; i++)
        {
            for (int j = 0; j < projectilesPerOffset; j++)
            {
                var instantiatedProjectile = Instantiate(unitPrefab, objTransform.position, objTransform.rotation);
                objTransform.localPosition += objTransform.right * spawnOffsets[j].x
                                              + objTransform.up * spawnOffsets[j].y
                                              + objTransform.forward * spawnOffsets[j].z;
                instantiatedProjectile.FireProjectile(unit.UnitStatsModifierManager.CurrentUnitStatsData[EUnitFloatStats.Damage], 
                    unit.Colliders, objTransform.forward, unit.UnitStatsModifierManager.CurrentUnitStatsData[EUnitFloatStats.MovementSpeed]);
            }
        }
    }
}
