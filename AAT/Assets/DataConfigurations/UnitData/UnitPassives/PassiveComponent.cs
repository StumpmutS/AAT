using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveComponent : ScriptableObject
{
    public abstract void ActivateComponent(UnitController unit);
}
