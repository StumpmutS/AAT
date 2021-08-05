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
            Debug.Log("left click");
            OnLeftCLick.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right click");
            OnRightClick.Invoke();
        }
    }

    private void CheckMouseMovement()
    {
        float verticalMouseAxis = Input.GetAxis("Mouse Y");
        if (verticalMouseAxis != 0)
        {
            Debug.Log("mouse y change");
            OnMouseYChange.Invoke(verticalMouseAxis);
        }
        float horizontalMouseAxis = Input.GetAxis("Mouse X");
        if (horizontalMouseAxis != 0)
        {
            Debug.Log("mouse x change");
            OnMouseXChange.Invoke(horizontalMouseAxis);
        }
    }
    
    private void CheckAxes()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        if (verticalAxis != 0)
        {
            Debug.Log("vertical pressed");
            OnVerticalAxis.Invoke(verticalAxis);
        }
        float horizontalAxis = Input.GetAxis("Horizontal");
        if (horizontalAxis != 0)
        {
            Debug.Log("horizontal pressed");
            OnHorizontalAxis.Invoke(horizontalAxis);
        }
        float jumpAxis = Input.GetAxis("Jump");
        if (jumpAxis != 0)
        {
            Debug.Log("jump axis pressed");
            OnJump.Invoke(jumpAxis);
        }
    }

    private void CheckLeftShift()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("left shift pressed");
            OnLeftShiftPressed.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("left shift up");
            OnLeftShiftEnd.Invoke();
        }
    }
    #endregion
}