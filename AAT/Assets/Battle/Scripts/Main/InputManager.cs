using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool leftShiftDown;

    public static event Action OnUpdate = delegate{ };

    public static event Action OnLeftCLickUp = delegate { };
    public static event Action OnLeftClickDown = delegate { };
    public static event Action OnRightClickDown = delegate { };

    public static event Action<float> OnMouseYChange = delegate { };
    public static event Action<float> OnMouseXChange = delegate { };

    public static event Action<float> OnMouseWheelScroll = delegate { };

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
    public static event Action<int> OnNumberKey7 = delegate { };

    public static event Action OnTPressed = delegate { };

    public static event Action OnPlus = delegate { };
    public static event Action OnMinus = delegate { };

    private void Update()
    {
        OnUpdate.Invoke();

        CheckMouseButtons();

        CheckMouseMovement();

        CheckMouseScroll();

        CheckAxes();

        CheckLeftShift();

        CheckNumbers();

        CheckAlphabetKeys();

        CheckSymbols();
    }

    #region Check Methods
    private void CheckMouseButtons()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftClickDown.Invoke();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnLeftCLickUp.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRightClickDown.Invoke();
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

    private void CheckMouseScroll()
    {
        float mouseScrollAmount = Input.mouseScrollDelta.y;
        if (mouseScrollAmount != 0)
        {
            OnMouseWheelScroll.Invoke(mouseScrollAmount);
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
            leftShiftDown = true;
            OnLeftShiftPressed.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            leftShiftDown = false;
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
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            OnNumberKey7.Invoke(7);
        }
    }

    private void CheckAlphabetKeys()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnTPressed.Invoke();
        }
    }

    private void CheckSymbols()
    {
        if (leftShiftDown && Input.GetKeyDown(KeyCode.Equals))
        {
            OnPlus.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            OnMinus.Invoke();
        }
    }
    #endregion
}