using Fusion;
using UnityEngine;

public class ArmoredHealth : Health, IArmor
{
    [Networked] private float currentArmorPercent { get; set; }
    
    private float _baseArmorPercent => stats.GetStat(EUnitFloatStats.BaseArmorPercent);
    private float _maxArmorPercent => stats.GetStat(EUnitFloatStats.MaxArmorPercent);

    protected override void Start()
    {
        base.Start();
        currentArmorPercent = _baseArmorPercent;
    }

    public override void CalculateDamageSeverity(ref float amount, ref int severity)
    {
        amount += amount * currentArmorPercent / 100;
        severity = Mathf.CeilToInt(severity * (1 - currentArmorPercent / 100));
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
