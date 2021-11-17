using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileComponentData : ScriptableObject
{
    public float ComponentTime;
    
    public abstract void ActivateComponent(GameObject from, GameObject hit, float damage);
    public virtual void DeactivateComponent(GameObject hit) { }
}
