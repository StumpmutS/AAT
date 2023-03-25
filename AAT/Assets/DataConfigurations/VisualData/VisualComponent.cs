using UnityEngine;

public abstract class VisualComponent : ScriptableObject
{
    [SerializeField] private float delay;
    public float Delay => delay;
    [SerializeField] private float duration;
    public float Duration => duration;

    public abstract void ActivateComponent(VisualInfo info);

    public virtual void DeactivateComponent(VisualInfo info) { }
}