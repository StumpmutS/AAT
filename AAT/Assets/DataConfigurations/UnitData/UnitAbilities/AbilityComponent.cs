using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityComponent : ScriptableObject
{
    public bool Repeat;
    public float ComponentDelay;
    public float ComponentDuration;

    public abstract void ActivateComponent(GameObject gameObject);
    public virtual void DeactivateComponent(GameObject gameObject) { }
}
