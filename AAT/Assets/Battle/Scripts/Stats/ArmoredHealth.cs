using UnityEngine;

public class ArmoredHealth : Health, IArmor
{
    private float baseArmorPercent => unitDataManager.GetStat(EUnitFloatStats.BaseArmorPercent);
    private float maxArmorPercent => unitDataManager.GetStat(EUnitFloatStats.MaxArmorPercent);

    private float currentArmorPercent;

    protected override void Start()
    {
        base.Start();
        currentArmorPercent = baseArmorPercent;
    }

    protected override void TakeDamage(float amount)
    {
        _currentHealth -= amount - (amount * (currentArmorPercent / 100));
        if (_currentHealth <= 0)
            Die();
    }

    public void ModifyArmor(float amount)
    {
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
        if (currentArmorPercent + amount >= maxArmorPercent)
            currentArmorPercent = maxArmorPercent;
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
