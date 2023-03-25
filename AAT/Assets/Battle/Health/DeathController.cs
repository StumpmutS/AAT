using System;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(IHealth))]
public class DeathController : NetworkBehaviour
{
    [Networked] private NetworkBool _dead { get; set; }
    
    [FormerlySerializedAs("unitCarcass")] [SerializeField] private GameObject carcass;

    private IHealth _healthController;

    public event Action OnDeath = delegate { };

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
        OnDeath.Invoke();
        if (carcass != null) Instantiate(carcass, transform.position, transform.rotation);
        gameObject.SetActive(false);
        if (!Runner.IsServer) return;
        
        Runner.Despawn(Object);
    }
}
