using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth;

    protected float currentHealth;

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
