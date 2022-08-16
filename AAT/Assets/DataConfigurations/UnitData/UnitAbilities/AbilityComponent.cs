using System.Collections;
using UnityEngine;

public abstract class AbilityComponent : StumpComponent
{
    public bool Repeat;
    [ShowIf(nameof(Repeat), true)]
    public float RepeatIntervals;
    public float ComponentDelay;
    public float ComponentDuration;
}
