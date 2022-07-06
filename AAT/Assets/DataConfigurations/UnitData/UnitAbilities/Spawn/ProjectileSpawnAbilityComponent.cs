using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Spawn/Projectile Spawn Ability Component")]
public class ProjectileSpawnAbilityComponent : AbilityComponent
{
    [SerializeField] private ProjectileController unitPrefab;
    [SerializeField] private int projectilesPerOffset;
    [SerializeField] private List<Vector3> spawnOffsets;
    [SerializeField] private bool useMoveSpeed;
    [SerializeField] private float alternateSpeed;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        var firerTransform = unit.transform;
        
        for (int i = 0; i < spawnOffsets.Count; i++)
        {
            for (int j = 0; j < projectilesPerOffset; j++)
            {
                var instantiatedProjectile = Instantiate(unitPrefab, firerTransform.position, firerTransform.rotation);
                instantiatedProjectile.transform.position += firerTransform.right * spawnOffsets[j].x
                                              + firerTransform.up * spawnOffsets[j].y
                                              + firerTransform.forward * spawnOffsets[j].z;
                instantiatedProjectile.FireProjectile(unit.Stats.GetStat(EUnitFloatStats.Damage), 
                    unit.Colliders, firerTransform.forward, useMoveSpeed ? unit.Stats.GetStat(EUnitFloatStats.MovementSpeed) : alternateSpeed);
            }
        }
    }
}
