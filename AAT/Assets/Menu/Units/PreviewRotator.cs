using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewRotator : MonoBehaviour
{
    [SerializeField] private Transform preview;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float inputRotationSpeed = 30;
    [SerializeField] private float speedLerpSpeed = 5;

    private bool _inputRotate;
    private float _prevMouseX;
    private float _currentSpeed;

    private void Awake()
    {
        _currentSpeed = rotationSpeed;
        BaseInputManager.OnLeftClickDown += BeginInputRotation;
        BaseInputManager.OnLeftCLickUp += EndInputRotation;
    }

    private void Update()
    {
        if (_inputRotate) InputRotate();
        else
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, rotationSpeed, speedLerpSpeed * Time.deltaTime);
            preview.Rotate(Vector3.up, _currentSpeed * Time.deltaTime);
        }
    }

    private void InputRotate()
    {
        var currentMouseX = Input.mousePosition.x;
        var diff = _prevMouseX - currentMouseX;
        _currentSpeed = diff * inputRotationSpeed;
        preview.Rotate(Vector3.up, _currentSpeed * Time.deltaTime);
        _prevMouseX = currentMouseX;
    }

    private void BeginInputRotation()
    {
        if (UIHoveredReference.Instance.OverUI()) return;
        
        _prevMouseX = Input.mousePosition.x;
        _inputRotate = true;
    }
    
    private void EndInputRotation() => _inputRotate = false;

    private void OnDestroy()
    {
        BaseInputManager.OnLeftClickDown -= BeginInputRotation;
        BaseInputManager.OnLeftCLickUp -= EndInputRotation;
    }
}
