using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Actions/Spawn/Projectile Spawn")]
public class ProjectileSpawnGameAction : AbilityGameAction
{
    [SerializeField] private ProjectileController projectilePrefab;
    [SerializeField] private int projectilesPerOffset;
    [SerializeField] private List<Vector3> spawnOffsets;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private bool useMoveSpeed;
    [SerializeField] private float alternateSpeed;

    public override void PerformAction(GameActionInfo info)
    {
        SpawnProjectiles(GetTransform(info.TransformChain), info.MainCaller);
    }

    private void SpawnProjectiles(Transform firerTransform, NetworkObject mainCaller)
    {
        for (int i = 0; i < spawnOffsets.Count; i++)
        {
            for (int j = 0; j < projectilesPerOffset; j++)
            {
                var instantiatedProjectile = mainCaller.Runner.Spawn(projectilePrefab, firerTransform.position,
                    firerTransform.rotation, mainCaller.InputAuthority);
                instantiatedProjectile.Init(mainCaller.GetComponent<TeamController>().GetTeamNumber());
                instantiatedProjectile.transform.position += firerTransform.right * spawnOffsets[j].x
                                                             + firerTransform.up * spawnOffsets[j].y
                                                             + firerTransform.forward * spawnOffsets[j].z;
                var stats = mainCaller.GetComponent<StatsManager>();
                instantiatedProjectile.FireProjectile(stats.GetModifiedStat(EUnitFloatStats.Damage) * damageMultiplier,
                    mainCaller.GetComponent<ColliderReference>().Colliders, firerTransform.forward,
                    useMoveSpeed ? stats.GetModifiedStat(EUnitFloatStats.MovementSpeed) : alternateSpeed);
            }
        }
    }
}
