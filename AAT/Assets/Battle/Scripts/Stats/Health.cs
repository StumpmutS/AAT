using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] protected UnitStatsModifierManager unitDataManager;
    private float maxHealth => unitDataManager.CurrentUnitStatsData[EUnitFloatStats.MaxHealth];

    public event Action<float> OnHealthChanged = delegate { };
    public event Action OnDie = delegate { };
    
    protected float currentHealth;
    private float currentHealthPercent;

    protected virtual void Awake()
    {
        unitDataManager.OnRefreshStats += RefreshHealth;
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        currentHealthPercent = currentHealth / maxHealth;
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
        currentHealthPercent = currentHealth / maxHealth;
        OnHealthChanged.Invoke(currentHealthPercent);
    }

    protected virtual void Heal(float amount)
    {
        if (currentHealth + amount > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amount;
    }

    protected virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }
    
    protected virtual void Die()
    {
        OnDie.Invoke();
    }

    private void RefreshHealth()
    {
        currentHealth = maxHealth * currentHealthPercent;
        OnHealthChanged.Invoke(currentHealthPercent);
    }
}
