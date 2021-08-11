using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeathController : MonoBehaviour
{
    private IHealth healthController;
    private Action<int, int> _callback;
    private int _unitIndex;
    private int _unitGroupIndex;

    private void Start()
    {
        healthController = GetComponent<IHealth>();
        healthController.OnDie += Die;
    }

    public void Setup(Action<int, int> callback, int unitGroupIndex, int unitIndex)
    {
        _callback = callback;
        _unitGroupIndex = unitGroupIndex;
        _unitIndex = unitIndex;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        _callback?.Invoke(_unitGroupIndex, _unitIndex);
    }
}
