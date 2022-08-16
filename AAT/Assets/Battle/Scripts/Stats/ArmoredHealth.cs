using Fusion;
using UnityEngine;

public class ArmoredHealth : Health, IArmor
{
    [Networked] private float currentArmorPercent { get; set; }
    
    private float _baseArmorPercent => unitDataManager.GetStat(EUnitFloatStats.BaseArmorPercent);
    private float _maxArmorPercent => unitDataManager.GetStat(EUnitFloatStats.MaxArmorPercent);

    protected override void Start()
    {
        base.Start();
        currentArmorPercent = _baseArmorPercent;
    }

    protected override void TakeDamage(float amount)
    {
        _currentHealth -= amount - (amount * (currentArmorPercent / 100));
        if (_currentHealth <= 0)
            Die();
    }

    public void ModifyArmor(float amount)
    {
        if (!Runner.IsServer) return;

        if (amount > 0)
        {
            AddArmor(amount);
        }
        else
        {
            RemoveArmor(Mathf.Abs(amount));
        }
    }

    private void AddArmor(float amount)
    {
        if (currentArmorPercent + amount >= _maxArmorPercent)
            currentArmorPercent = _maxArmorPercent;
        else
            currentArmorPercent += amount;
    }

    private void RemoveArmor(float amount)
    {
        if (currentArmorPercent - amount <= 0)
            currentArmorPercent = 0;
        else
            currentArmorPercent -= amount;
    }
}
