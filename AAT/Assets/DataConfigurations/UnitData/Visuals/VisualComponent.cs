using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisualComponent : StumpComponent
{
    [SerializeField] private float delay;
    public float Delay => delay;
    [SerializeField] private float duration;
    public float Duration => duration;
}
