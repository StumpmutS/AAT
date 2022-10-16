using System;
using UnityEngine;

public interface IHealth
{
    event Action<float> OnHealthPercentChanged;
    event Action OnDie;
    void ModifyHealth(float amount, int maxSeverity = 1);
}
