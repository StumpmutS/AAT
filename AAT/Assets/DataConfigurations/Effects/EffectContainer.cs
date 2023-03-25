using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectContainer : MonoBehaviour
{
    [SerializeField] private List<StumpEffect> startEffects;

    private List<StumpEffect> _effects = new();

    private void Awake()
    {
        foreach (var effect in startEffects)
        {
            AddEffect(effect);
        }
    }

    public void AddEffect(StumpEffect effect)
    {
        _effects.Add(effect);
    }

    public void RemoveEffect(StumpEffect effect)
    {
        _effects.Remove(effect);
    }

    public IEnumerable<T> GetEffects<T>() where T : StumpEffect
    {
        return _effects.OfType<T>();
    }
}