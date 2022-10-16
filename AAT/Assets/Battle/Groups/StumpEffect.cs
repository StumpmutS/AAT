using UnityEngine;

public class StumpEffect : ScriptableObject
{
    [SerializeField] private EEffectType effectType;
    public EEffectType EffectType => effectType;
}