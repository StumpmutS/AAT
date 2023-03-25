using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Actions/AOE/Sphere AOE Damage")]
public class SphereAoeDamageAbilityGameAction : AoeDamageAbilityGameActionData
{
    [SerializeField] private float radius;
    
    protected override List<LagCompensatedHit> GetHits(Transform transform, NetworkObject caller)
    {
        List<LagCompensatedHit> hits = new();
        caller.Runner.LagCompensation.OverlapSphere(transform.position, radius, caller.InputAuthority, hits);
        return hits;
    }
}