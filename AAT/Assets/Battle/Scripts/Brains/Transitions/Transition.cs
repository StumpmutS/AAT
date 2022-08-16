using Fusion;
using UnityEngine;

public abstract class Transition : ScriptableObject
{
    public bool Any;
    [ShowIf(nameof(Any), false)]
    public ComponentState From;
    public ComponentState To;

    public abstract bool Decision(UnitController unit);
}
