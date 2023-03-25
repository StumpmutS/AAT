using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class AoeDamageAbilityGameActionData : AbilityGameAction
{
    [SerializeField] private float damage;

    public override void PerformAction(GameActionInfo info)
    {
        var transform = GetTransform(info.TransformChain);
        if (transform == null) return;
        DamageFrom(info.MainCaller, transform);
    }

    private void DamageFrom(NetworkObject caller, Transform transform)
    {
        damage = -Mathf.Abs(damage);
        List<LagCompensatedHit> hits = GetHits(transform, caller);
        
        foreach (var hit in hits)
        {
            var attackables = hit.GameObject.GetComponents<IAttackable>();
            if (attackables.Length < 1) continue;

            foreach (var attackable in attackables)
            {
                attackable.TakeAttack(damage);
            }
        }
    }

    protected abstract List<LagCompensatedHit> GetHits(Transform transform, NetworkObject caller);
}
