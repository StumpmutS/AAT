using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Actions/AOE/Cone AOE Damage")]
public class ConeAoeDamageAbilityGameAction : AoeDamageAbilityGameActionData
{
    [SerializeField] private float length;
    [SerializeField] private float radius;
    
    protected override List<LagCompensatedHit> GetHits(Transform transform, NetworkObject caller)
    {
        List<LagCompensatedHit> hits = new();
        caller.Runner.LagCompensation.OverlapBox(transform.position + transform.forward * length / 2,
            new Vector3(radius, radius, length), Quaternion.Euler(transform.forward), caller.InputAuthority, hits);
        return hits;
    }
}