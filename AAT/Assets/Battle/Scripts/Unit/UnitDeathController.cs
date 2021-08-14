using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHealth))]
public class UnitDeathController : MonoBehaviour
{
    private IHealth healthController;

    public event Action OnUnitDeath = delegate { };

    private void Start()
    {
        healthController = GetComponent<IHealth>();
        healthController.OnDie += Die;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        OnUnitDeath.Invoke();
    }
}
