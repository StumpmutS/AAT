using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredDecayingHealth : ArmoredHealth
{
    [Tooltip("Health percentage lost per second")]
    [SerializeField] private float decayRate;

    public override void FixedUpdateNetwork()
    {
        ModifyHealth(-Mathf.Abs(MaxHealth * decayRate / 100 * Runner.DeltaTime), null, new AttackDecalInfo());
    }
}
