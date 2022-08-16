using System;
using Fusion;
using UnityEngine;
using Utility.Scripts;

public class BaseInputManager : MonoBehaviour
{
    #region Clicks
    public static Vector3 RightClickPosition;
    public static Vector3 RightClickDirection;
    public static Vector3 LeftClickPosition;
    public static Vector3 LeftClickDirection;

    private void SetRightClick()
    {
        RightClickPosition.SetToCursorToWorldPosition(LayerManager.Instance.GroundLayer);
        var ray = MainCameraRef.Cam.ScreenPointToRay(Input.mousePosition);
        RightClickDirection = ray.direction;
    }

    private void SetLeftClick()
    {
        LeftClickPosition.SetToCursorToWorldPosition(LayerManager.Instance.GroundLayer);
        var ray = MainCameraRef.Cam.ScreenPointToRay(Input.mousePosition);
        LeftClickDirection = ray.direction;
    }
    #endregion
    
    private bool _leftShiftDown;

    public static event Action OnUpdate = delegate{ };

    public static event Action OnLeftClickDown = delegate { };
    public static event Action OnLeftCLickUp = delegate { };
    public static event Action OnRightClickDown = delegate { };
    public static event Action OnRightClickUp = delegate { };

    public static event Action<float> OnMouseYChange = delegate { };
    public static event Action<float> OnMouseXChange = delegate { };

    public static event Action<float> OnMouseWheelScroll = delegate { };
    public static event Action OnMouseWheelDown = delegate { };
    public static event Action OnMouseWheelUp = delegate { };

    public static event Action<float> OnVerticalAxis = delegate { };
    public static event Action<float> OnHorizontalAxis = delegate { };
    public static event Action<float> OnJump = delegate { };

    public static event Action OnLeftShiftPressed = delegate { };
    public static event Action OnLeftShiftEnd = delegate { };

    public static event Action<int> OnAlpha0 = delegate { };
    public static event Action<int> OnAlpha1 = delegate { };
    public static event Action<int> OnAlpha2 = delegate { };
    public static event Action<int> OnAlpha3 = delegate { };
    public static event Action<int> OnAlpha4 = delegate { };
    public static event Action<int> OnAlpha5 = delegate { };
    public static event Action<int> OnAlpha6 = delegate { };
    public static event Action<int> OnAlpha7 = delegate { };
    public static event Action<int> OnAlpha8 = delegate { };
    public static event Action<int> OnAlpha9 = delegate { };

    public static event Action OnTPressed = delegate { };
    public static event Action OnRPressed = delegate { };

    public static event Action OnPlus = delegate { };
    public static event Action OnMinus = delegate { };
    
    private void Update()
    {
        OnUpdate.Invoke();
        
        CheckMouseButtons();
        
        //CheckNumbers();

        CheckMouseMovement();

        CheckMouseScroll();

        CheckAxes();

        CheckLeftShift();
        
        CheckAlphabetKeys();

        CheckSymbols();
    }

    #region Check Methods
    private void CheckMouseButtons()
    {
        if (!UIHoveredReference.Instance.OverUI())
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetLeftClick();
                OnLeftClickDown.Invoke();
            }
            if (Input.GetMouseButton(0))
            {
                SetLeftClick();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnLeftCLickUp.Invoke();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetRightClick();
            OnRightClickDown.Invoke();
        }
        if (Input.GetMouseButton(1))
        {
            SetRightClick();
        }
        if (Input.GetMouseButtonUp(1))
        {
            OnRightClickUp.Invoke();
        }
        if (Input.GetMouseButtonDown(2))
        {
            OnMouseWheelDown.Invoke();
        }
        if (Input.GetMouseButtonUp(2))
        {
            OnMouseWheelUp.Invoke();
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
            _leftShiftDown = true;
            OnLeftShiftPressed.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _leftShiftDown = false;
            OnLeftShiftEnd.Invoke();
        }
    }

    private void CheckNumbers(NetworkedInputData input)
    {/*
        if ((input.Buttons & NetworkedInputMapping.ALPHA0_DOWN) != 0)
        {
            OnAlpha0.Invoke(0);
        }
        if ((input.Buttons & NetworkedInputMapping.ALPHA1_DOWN) != 0)
        {
            OnAlpha1.Invoke(1);
        }
        if ((input.Buttons & NetworkedInputMapping.ALPHA2_DOWN) != 0)
        {
            OnAlpha2.Invoke(2);
        }
        if ((input.Buttons & NetworkedInputMapping.ALPHA3_DOWN) != 0)
        {
            OnAlpha3.Invoke(3);
        }
        if ((input.Buttons & NetworkedInputMapping.ALPHA4_DOWN) != 0)
        {
            OnAlpha4.Invoke(4);
        }
        if ((input.Buttons & NetworkedInputMapping.ALPHA5_DOWN) != 0)
        {
            OnAlpha5.Invoke(5);
        }
        if ((input.Buttons & NetworkedInputMapping.ALPHA6_DOWN) != 0)
        {
            OnAlpha6.Invoke(6);
        }
        if ((input.Buttons & NetworkedInputMapping.ALPHA7_DOWN) != 0)
        {
            OnAlpha7.Invoke(7);
        }
        if ((input.Buttons & NetworkedInputMapping.ALPHA8_DOWN) != 0)
        {
            OnAlpha8.Invoke(8);
        }
        if ((input.Buttons & NetworkedInputMapping.ALPHA9_DOWN) != 0)
        {
            OnAlpha9.Invoke(9);
        }*/
    }

    private void CheckAlphabetKeys()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnTPressed.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRPressed.Invoke();
        }
    }

    private void CheckSymbols()
    {
        if (_leftShiftDown && Input.GetKeyDown(KeyCode.Equals))
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