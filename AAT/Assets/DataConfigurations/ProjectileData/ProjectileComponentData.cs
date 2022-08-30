using UnityEngine;

public abstract class ProjectileComponentData : ScriptableObject
{
    public float ComponentTime;
    
    public abstract void ActivateComponent(ProjectileController from, GameObject hit, float damage);
    public virtual void DeactivateComponent(GameObject hit) { }
}
