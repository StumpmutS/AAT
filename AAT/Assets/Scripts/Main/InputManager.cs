using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action OnLeftCLick = delegate { };
    public static event Action<Vector3> OnRightClick = delegate { };

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("left click");
            OnLeftCLick.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit)) {
                Vector3 cursorToWorld = hit.point;
                OnRightClick.Invoke(cursorToWorld);
            }
        }
    }
}
