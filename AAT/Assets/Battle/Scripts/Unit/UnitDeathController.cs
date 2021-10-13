using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHealth))]
public class UnitDeathController : MonoBehaviour
{
    [SerializeField] GameObject unitCarcass;

    private IHealth healthController;

    public event Action OnUnitDeath = delegate { };

    private void Start()
    {
        healthController = GetComponent<IHealth>();
        healthController.OnDie += Die;
    }

    private void Die()
    {
        if (unitCarcass != null) Instantiate(unitCarcass, transform.position, transform.rotation);
        OnUnitDeath.Invoke();
        gameObject.SetActive(false);
    }
}
