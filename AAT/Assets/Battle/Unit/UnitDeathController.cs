using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(IHealth))]
public class UnitDeathController : NetworkBehaviour
{
    [Networked] private NetworkBool _dead { get; set; }
    
    [SerializeField] private GameObject unitCarcass;

    private IHealth _healthController;

    public event Action OnUnitDeath = delegate { };

    private void Start()
    {
        _healthController = GetComponent<IHealth>();
        _healthController.OnDie += Die;
    }

    public void Die()
    {
        if (_dead) return;
        _dead = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (!_dead) return;
        OnUnitDeath.Invoke();
        if (unitCarcass != null) Instantiate(unitCarcass, transform.position, transform.rotation);
        gameObject.SetActive(false);
        if (!Runner.IsServer) return;
        
        Runner.Despawn(Object);
    }
}
