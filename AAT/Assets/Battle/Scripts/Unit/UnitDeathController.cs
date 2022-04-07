using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHealth))]
public class UnitDeathController : MonoBehaviour
{
    [SerializeField] private GameObject unitCarcass;

    private bool _dead = false;
    private IHealth healthController;

    public event Action OnUnitDeath = delegate { };

    private void Start()
    {
        healthController = GetComponent<IHealth>();
        healthController.OnDie += Die;
    }

    public void Die()
    {
        if (_dead) return;
        _dead = true;
        if (unitCarcass != null) Instantiate(unitCarcass, transform.position, transform.rotation);
        OnUnitDeath.Invoke();
        gameObject.SetActive(false);
    }
}
