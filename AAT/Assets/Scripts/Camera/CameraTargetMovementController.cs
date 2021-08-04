using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetMovementController : MonoBehaviour
{
    [SerializeField] private float verticalMoveSpeed;
    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float verticalRotationSpeed;
    [SerializeField] private float horizontalRotationSpeed;
    [SerializeField] private float upDownSpeed;
    [SerializeField] private float boostSpeedMultiplier;

    private float currentVerticalRotation;
    private float currentHorizontalRotation;
    private bool boostActive;

    private void Awake()
    {
        currentVerticalRotation = -transform.eulerAngles.x;
        currentHorizontalRotation = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;

        InputManager.OnVerticalAxis += MoveTargetVertical;
        InputManager.OnHorizontalAxis += MoveTargetHorizontal;
        InputManager.OnJump += MoveTargetUpDown;

        InputManager.OnMouseYChange += RotateTargetVertical;
        InputManager.OnMouseXChange += RotateTargetHorizontal;

        InputManager.OnLeftShiftPressed += ActivateBoost;
        InputManager.OnLeftShiftEnd += DeactivateBoost;
    }


    private void MoveTargetVertical(float inputAmount)
    {
        if (!boostActive)
            transform.position += transform.forward * inputAmount * verticalMoveSpeed * Time.deltaTime;
        else
            transform.position += transform.forward * inputAmount * verticalMoveSpeed * boostSpeedMultiplier * Time.deltaTime;
    }

    private void MoveTargetHorizontal(float inputAmount)
    {
        if (!boostActive)
            transform.position += transform.right * inputAmount * horizontalMoveSpeed * Time.deltaTime;
        else
            transform.position += transform.right * inputAmount * horizontalMoveSpeed * boostSpeedMultiplier * Time.deltaTime;
    }

    private void MoveTargetUpDown(float inputAmount)
    {
        if (!boostActive)
            transform.position += transform.up * inputAmount * upDownSpeed * Time.deltaTime;
        else
            transform.position += transform.up * inputAmount * upDownSpeed * boostSpeedMultiplier * Time.deltaTime;
    }

    private void RotateTargetVertical(float inputAmount)
    {
        currentVerticalRotation += inputAmount * Time.deltaTime * verticalRotationSpeed;
        transform.rotation = Quaternion.Euler(-currentVerticalRotation, currentHorizontalRotation, 0);
    }

    private void RotateTargetHorizontal(float inputAmount)
    {
        currentHorizontalRotation += inputAmount * Time.deltaTime * horizontalRotationSpeed;
        transform.rotation = Quaternion.Euler(-currentVerticalRotation, currentHorizontalRotation, 0);
    }

    private void ActivateBoost()
    {
        boostActive = true;
    }

    private void DeactivateBoost()
    {
        boostActive = false;
    }
}
