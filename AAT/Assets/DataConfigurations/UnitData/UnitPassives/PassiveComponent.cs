using UnityEngine;

public abstract class PassiveComponent : ScriptableObject
{
    public abstract void ActivateComponent(UnitController unit);
}
