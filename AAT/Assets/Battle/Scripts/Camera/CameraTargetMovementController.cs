using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetMovementController : MonoBehaviour
{
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxMove;
    [SerializeField] private float heightMultiplier;
    [SerializeField] private float minMultiplier;
    private float HeightMultiplier => Mathf.Max(transform.position.y, maxHeight * minMultiplier) * heightMultiplier;
    
    [SerializeField] private float verticalMoveSpeed;
    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float verticalRotationSpeed;
    [SerializeField] private float horizontalRotationSpeed;
    [SerializeField] private float upDownSpeed;
    [SerializeField] private float boostSpeedMultiplier;
    
    private bool _boostActive;
    private bool _wheelDown;

    private void Awake()
    {
        InputManager.OnVerticalAxis += MoveTargetVertical;
        InputManager.OnHorizontalAxis += MoveTargetHorizontal;

        InputManager.OnMouseYChange += RotateTargetVertical;
        InputManager.OnMouseXChange += RotateTargetHorizontal;

        InputManager.OnLeftShiftPressed += ActivateBoost;
        InputManager.OnLeftShiftEnd += DeactivateBoost;

        InputManager.OnMouseWheelDown += WheelDown;
        InputManager.OnMouseWheelUp += WheelUp;

        InputManager.OnMouseWheelScroll += MoveTargetUpDown;
    }

    private void MoveTargetVertical(float inputAmount)
    {
        if (_boostActive)
            transform.Translate(new Vector3(transform.forward.x, 0, transform.forward.z) * inputAmount * HeightMultiplier * verticalMoveSpeed * boostSpeedMultiplier * Time.deltaTime, Space.World);
        else
            transform.Translate(new Vector3(transform.forward.x, 0, transform.forward.z) * inputAmount * HeightMultiplier * verticalMoveSpeed * Time.deltaTime, Space.World);
        CheckMaxMove();
    }

    private void MoveTargetHorizontal(float inputAmount)
    {
        if (_boostActive)
            transform.Translate(transform.right * inputAmount * HeightMultiplier * horizontalMoveSpeed * boostSpeedMultiplier * Time.deltaTime, Space.World);
        else
            transform.Translate(transform.right * inputAmount * HeightMultiplier * horizontalMoveSpeed * Time.deltaTime, Space.World);
        CheckMaxMove();
    }

    private void CheckMaxMove()
    {
        if (transform.position.x > maxMove)
            transform.position = new Vector3(maxMove, transform.position.y, transform.position.z);
        if (transform.position.x < -maxMove)
            transform.position = new Vector3(-maxMove, transform.position.y, transform.position.z);
        if (transform.position.z > maxMove)
            transform.position = new Vector3(transform.position.x, transform.position.y, maxMove);
        if (transform.position.z < -maxMove)
            transform.position = new Vector3(transform.position.x, transform.position.y, -maxMove);
    }

    private void MoveTargetUpDown(float inputAmount)
    {
        transform.Translate(Vector3.up * -inputAmount * HeightMultiplier * upDownSpeed * Time.deltaTime, Space.World);

        if (transform.position.y > maxHeight)
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
        if (transform.position.y < minHeight)
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
    }

    private void RotateTargetVertical(float inputAmount)
    {
        if (!_wheelDown) return;
        transform.Rotate(transform.right, -(inputAmount * Time.deltaTime * verticalRotationSpeed), Space.World);
    }
    
    private void RotateTargetHorizontal(float inputAmount)
    {
        if (!_wheelDown) return;
        transform.Rotate(Vector3.up, inputAmount * Time.deltaTime * horizontalRotationSpeed, Space.World);
    }
    
    private void ActivateBoost()
    {
        _boostActive = true;
    }

    private void DeactivateBoost()
    {
        _boostActive = false;
    }
    
    private void WheelDown()
    {
        _wheelDown = true;
    }
    
    private void WheelUp()
    {
        _wheelDown = false;
    }
}
