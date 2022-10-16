using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour, IHealth, IAttackable
{
    [Networked(OnChanged = nameof(OnHealthPercentChange))] 
    private float _currentHealthPercent { get; set; }
    public static void OnHealthPercentChange(Changed<Health> changed)
    {
        changed.Behaviour.OnHealthPercentChanged.Invoke(changed.Behaviour._currentHealthPercent);
    }
    
    [SerializeField] protected StatsManager stats;
    public float MaxHealth => stats.GetStat(EUnitFloatStats.MaxHealth);
    
    protected float _currentHealth;
    
    public event Action<float> OnHealthPercentChanged = delegate { };
    public event Action OnDie = delegate { };

    protected virtual void Awake()
    {
        stats.OnRefreshStats += RefreshHealth;
    }

    protected virtual void Start()
    {
        _currentHealth = MaxHealth;
        _currentHealthPercent = _currentHealth / MaxHealth;
    }

    public void ModifyHealth(float amount, int maxSeverity = 1)
    {
        if (amount > 0)
        {
            CalculateHealingSeverity(ref amount, ref maxSeverity);
            if (!Runner.IsServer) return;
            Heal(amount);
        }
        else
        {
            CalculateDamageSeverity(ref amount, ref maxSeverity);
            if (!Runner.IsServer) return;
            TakeDamage(Mathf.Abs(amount));
        }
        
        _currentHealthPercent = _currentHealth / MaxHealth;
    }

    public virtual void CalculateDamageSeverity(ref float amount, ref int severity) { }
    
    public virtual void CalculateHealingSeverity(ref float amount, ref int severity) { }

    private void Heal(float amount)
    {
        if (_currentHealth + amount > MaxHealth)
            _currentHealth = MaxHealth;
        else
            _currentHealth += amount;
    }

    private void TakeDamage(float amount)
    {
    }
    
    protected virtual void Die()
    {
        OnDie.Invoke();
    }

    private void RefreshHealth(StatsManager stats)
    {
        _currentHealth = MaxHealth * _currentHealthPercent;
    }

    public void TakeAttack(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
            Die();
    }
}

public interface IAttackable
{
    public void TakeAttack(float amount);
}