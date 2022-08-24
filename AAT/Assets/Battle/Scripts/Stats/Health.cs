using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour, IHealth
{
    [Networked(OnChanged = nameof(OnHealthPercentChange))] 
    private float _currentHealthPercent { get; set; }
    public static void OnHealthPercentChange(Changed<Health> changed)
    {
        changed.Behaviour.OnHealthPercentChanged.Invoke(changed.Behaviour._currentHealthPercent);
    }
    
    [SerializeField] protected UnitStatsModifierManager unitDataManager;
    public float MaxHealth => unitDataManager.GetStat(EUnitFloatStats.MaxHealth);
    
    protected float _currentHealth;
    protected VisualsHandler _visualsHandler;
    
    public event Action<float> OnHealthPercentChanged = delegate { };
    public event Action OnDie = delegate { };

    protected virtual void Awake()
    {
        _visualsHandler = GetComponent<VisualsHandler>();
        unitDataManager.OnRefreshStats += RefreshHealth;
    }

    protected virtual void Start()
    {
        _currentHealth = MaxHealth;
        _currentHealthPercent = _currentHealth / MaxHealth;
    }

    public void ModifyHealth(float amount, DecalImage decal, AttackDecalInfo info)
    {
        if (amount > 0)
        {
            HealDecal(Mathf.Abs(amount), decal, info);
            if (!Runner.IsServer) return;
            Heal(amount);
        }
        else
        {
            DamagedDecal(Mathf.Abs(amount), decal, info);
            if (!Runner.IsServer) return;
            TakeDamage(Mathf.Abs(amount));
        }
        
        _currentHealthPercent = _currentHealth / MaxHealth;
    }

    protected virtual void Heal(float amount)
    {
        if (_currentHealth + amount > MaxHealth)
            _currentHealth = MaxHealth;
        else
            _currentHealth += amount;
    }

    protected virtual void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
            Die();
    }

    protected virtual void DamagedDecal(float amount, DecalImage decal, AttackDecalInfo info)
    {
        if (decal != null) _visualsHandler.CreateDecal(decal, info);
    }

    protected virtual void HealDecal(float amount, DecalImage decal, AttackDecalInfo info)
    {
        if (decal != null) _visualsHandler.CreateDecal(decal, info);
    }
    
    protected virtual void Die()
    {
        OnDie.Invoke();
    }

    private void RefreshHealth(UnitStatsModifierManager stats)
    {
        _currentHealth = MaxHealth * _currentHealthPercent;
    }
}
