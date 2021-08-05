using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    event Action<float> OnHealthChanged;
    void ModifyHealth(float amount);
}
