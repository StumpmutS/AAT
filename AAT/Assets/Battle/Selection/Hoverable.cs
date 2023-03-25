using System;
using UnityEngine;
using UnityEngine.Events;

public class Hoverable : MonoBehaviour
{
    public UnityEvent OnHover;
    public UnityEvent OnHoverStop;

    public virtual void Hover()
    {
        OnHover.Invoke();
    }

    public virtual void StopHover()
    {
        OnHoverStop.Invoke();
    }
}