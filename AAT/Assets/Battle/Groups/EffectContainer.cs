using System.Collections.Generic;
using UnityEngine;

public class EffectContainer : MonoBehaviour
{
    [SerializeField] private List<StumpEffect> startEffects;

    private Dictionary<EEffectType, List<StumpEffect>> _effects;

    public void AddEffect(StumpEffect effect)
    {
        if (!_effects.ContainsKey(effect.EffectType)) _effects[effect.EffectType] = new List<StumpEffect>();
        _effects[effect.EffectType].Add(effect);
    }

    public IEnumerable<StumpEffect> GetEffects(EEffectType effectType)
    {
        return _effects[effectType];
    }
}