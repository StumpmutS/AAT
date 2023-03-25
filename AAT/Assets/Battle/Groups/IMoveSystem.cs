using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveSystem
{
    public event Action OnPathFinished;
    
    public void SetTarget(StumpTarget target);
    public void Warp(StumpTarget target);
    public void Stop();
    public void Enable();
    public void Disable();
}