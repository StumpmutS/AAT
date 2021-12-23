using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityComponent : ScriptableObject
{
    public bool Repeat;
    public float RepeatIntervals;
    public float ComponentDelay;
    public float ComponentDuration;

    public event Action OnCompletion = delegate { };

    public abstract void ActivateComponent(UnitController unit, Vector3 point = default);
    public virtual void DeactivateComponent(UnitController unit) { }
}
