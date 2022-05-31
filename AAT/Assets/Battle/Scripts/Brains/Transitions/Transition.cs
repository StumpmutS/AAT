using UnityEngine;

public abstract class Transition : ScriptableObject
{
    public bool Any;
    public ComponentState From;
    public ComponentState To;

    public abstract bool Decision(UnitController unit);
}
