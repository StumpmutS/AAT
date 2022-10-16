using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class HoverController : SimulationBehaviour, IMoveSystem
{
    private float _speed;

    public event Action OnPathFinished;

    public void Move(StumpTarget target)
    {
        throw new System.NotImplementedException();
    }

    public void Follow(StumpTarget target)
    {
        throw new System.NotImplementedException();
    }

    public void Stop()
    {
        throw new System.NotImplementedException();
    }

    public void Enable()
    {
        throw new NotImplementedException();
    }

    public void Disable()
    {
        throw new NotImplementedException();
    }
}
