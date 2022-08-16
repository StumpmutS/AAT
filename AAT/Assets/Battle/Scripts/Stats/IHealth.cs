using System;

public interface IHealth
{
    event Action<float> OnHealthPercentChanged;
    event Action OnDie;
    void ModifyHealth(float amount);
}
