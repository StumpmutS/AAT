using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalSetSpring : DecalComponent
{
    [SerializeField] private SpringController spring;
    
    public override void Activate()
    {
        spring.SetTarget(1);
    }

    public override void Deactivate()
    {
        spring.SetTarget(0);
    }
}
