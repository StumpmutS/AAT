using System;

public interface IHealth
{
    event Action<float> OnHealthChanged;
    event Action OnDie;
    void ModifyHealth(float amount);
}
