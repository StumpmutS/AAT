using Fusion;
using UnityEngine;

public abstract class Restriction : ScriptableObject
{
    public abstract bool CheckRestriction(UnitController unit);
}