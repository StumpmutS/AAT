using System;
using UnityEngine;
using UnityEngine.Events;

public class Hoverable : MonoBehaviour
{
    public UnityEvent OnHover;
    public UnityEvent OnHoverStop;

    public void Hover()
    {
        OnHover.Invoke();
    }

    public void StopHover()
    {
        OnHoverStop.Invoke();
    }
}