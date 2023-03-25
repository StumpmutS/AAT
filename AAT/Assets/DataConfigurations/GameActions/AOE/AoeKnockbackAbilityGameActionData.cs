using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Actions/AOE/AOE Knockback")]
public class AoeKnockbackAbilityGameActionData : AbilityGameAction
{
    [SerializeField] private float knockbackRadius;
    [SerializeField] private float knockbackPower;

    public override void PerformAction(GameActionInfo info)
    {
        KnockbackFrom(info.MainCaller, GetTransform(info.TransformChain).position);
    }

    private void KnockbackFrom(NetworkObject caller, Vector3 point)
    {
        List<LagCompensatedHit> hits = new();
        caller.Runner.LagCompensation.OverlapSphere(point, knockbackRadius, caller.InputAuthority, hits);
        foreach (var hit in hits)
        {
            var knockables = hit.GameObject.GetComponents<IKnockable>();
            
            if (knockables.Length < 1) continue;

            var direction = hit.Point - point;
            
            foreach (var knockable in knockables)
            { 
                knockable.Knockback(direction, knockbackPower);
            }
        }
    }
}