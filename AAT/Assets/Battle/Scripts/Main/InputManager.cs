using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action OnLeftCLick = delegate { };
    public static event Action OnRightClick = delegate { };

    public static event Action<float> OnMouseYChange = delegate { };
    public static event Action<float> OnMouseXChange = delegate { };

    public static event Action<float> OnVerticalAxis = delegate { };
    public static event Action<float> OnHorizontalAxis = delegate { };
    public static event Action<float> OnJump = delegate { };

    public static event Action OnLeftShiftPressed = delegate { };
    public static event Action OnLeftShiftEnd = delegate { };

    private void Update()
    {
        CheckMouseButtons();

        CheckMouseMovement();

        CheckAxes();

        CheckLeftShift();
    }

    #region Check Methods
    private void CheckMouseButtons()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftCLick.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRightClick.Invoke();
        }
    }

    private void CheckMouseMovement()
    {
        float verticalMouseAxis = Input.GetAxis("Mouse Y");
        if (verticalMouseAxis != 0)
        {
            OnMouseYChange.Invoke(verticalMouseAxis);
        }
        float horizontalMouseAxis = Input.GetAxis("Mouse X");
        if (horizontalMouseAxis != 0)
        {
            OnMouseXChange.Invoke(horizontalMouseAxis);
        }
    }
    
    private void CheckAxes()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        if (verticalAxis != 0)
        {
            OnVerticalAxis.Invoke(verticalAxis);
        }
        float horizontalAxis = Input.GetAxis("Horizontal");
        if (horizontalAxis != 0)
        {
            OnHorizontalAxis.Invoke(horizontalAxis);
        }
        float jumpAxis = Input.GetAxis("Jump");
        if (jumpAxis != 0)
        {
            OnJump.Invoke(jumpAxis);
        }
    }

    private void CheckLeftShift()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            OnLeftShiftPressed.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            OnLeftShiftEnd.Invoke();
        }
    }
    #endregion
}