using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] protected UnitStatsModifierManager unitDataManager;
    private float _maxHealth => unitDataManager.GetStat(EUnitFloatStats.MaxHealth);

    public event Action<float> OnHealthChanged = delegate { };
    public event Action OnDie = delegate { };
    
    protected float _currentHealth;
    private float _currentHealthPercent;

    protected virtual void Awake()
    {
        unitDataManager.OnRefreshStats += RefreshHealth;
    }

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
        _currentHealthPercent = _currentHealth / _maxHealth;
    }

    public void ModifyHealth(float amount)
    {
        if (amount > 0)
        {
            Heal(amount);
        }
        else
        {
            TakeDamage(Mathf.Abs(amount));
        }
        _currentHealthPercent = _currentHealth / _maxHealth;
        OnHealthChanged.Invoke(_currentHealthPercent);
    }

    protected virtual void Heal(float amount)
    {
        if (_currentHealth + amount > _maxHealth)
            _currentHealth = _maxHealth;
        else
            _currentHealth += amount;
    }

    protected virtual void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
            Die();
    }
    
    protected virtual void Die()
    {
        OnDie.Invoke();
    }

    private void RefreshHealth()
    {
        _currentHealth = _maxHealth * _currentHealthPercent;
        OnHealthChanged.Invoke(_currentHealthPercent);
    }
}
