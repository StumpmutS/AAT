using UnityEngine;

public abstract class SpringListener : MonoBehaviour
{
    [SerializeField] protected bool useSetValue;
    [SerializeField, ShowIf(nameof(useSetValue), false)] protected float minMultiplier, maxMultiplier;
    
    public abstract void HandleSpringValue(float amount, float target);
}
