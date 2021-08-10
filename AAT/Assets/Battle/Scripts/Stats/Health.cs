using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] protected UnitStatsData unitData;
    private float maxHealth => unitData.MaxHealth;

    public event Action<float> OnHealthChanged;
    
    protected float currentHealth;
    private float currentHealthPercent;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public void ModifyHealth(float amount)
    {
        Debug.Log($"health before modify: {currentHealth}");
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
        Debug.Log($"health after modify: {currentHealth}");
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
        gameObject.SetActive(false);
    }
}
