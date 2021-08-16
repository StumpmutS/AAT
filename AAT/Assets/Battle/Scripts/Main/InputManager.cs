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

    public static event Action<int> OnNumberKey0 = delegate { };
    public static event Action<int> OnNumberKey1 = delegate { };
    public static event Action<int> OnNumberKey2 = delegate { };
    public static event Action<int> OnNumberKey3 = delegate { };
    public static event Action<int> OnNumberKey4 = delegate { };
    public static event Action<int> OnNumberKey5 = delegate { };
    public static event Action<int> OnNumberKey6 = delegate { };

    private void Update()
    {
        CheckMouseButtons();

        CheckMouseMovement();

        CheckAxes();

        CheckLeftShift();

        CheckNumbers();
    }

    #region Check Methods
    private void CheckMouseButtons()
    {
        if (Input.GetMouseButtonUp(0))
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

    private void CheckNumbers()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            OnNumberKey0.Invoke(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            OnNumberKey1.Invoke(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            OnNumberKey2.Invoke(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            OnNumberKey3.Invoke(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            OnNumberKey4.Invoke(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            OnNumberKey5.Invoke(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            OnNumberKey6.Invoke(6);
        }
    }
    #endregion
}