using System.Collections.Generic;
using UnityEngine;

public abstract class StumpComponent : ScriptableObject
{
    [SerializeField] private List<Restriction> restrictions;
    public List<Restriction> Restrictions => restrictions;

    public abstract void ActivateComponent(UnitController unit, Vector3 point = default);
    public virtual void DeactivateComponent(UnitController unit) { }
}
