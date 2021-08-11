using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    event Action<float> OnHealthChanged;
    event Action OnDie;
    void ModifyHealth(float amount);
}
