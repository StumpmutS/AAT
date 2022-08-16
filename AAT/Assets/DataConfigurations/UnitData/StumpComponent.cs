using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StumpComponent : ScriptableObject
{
    public abstract void ActivateComponent(UnitController unit, Vector3 point = default);
    public virtual void DeactivateComponent(UnitController unit) { }
}
