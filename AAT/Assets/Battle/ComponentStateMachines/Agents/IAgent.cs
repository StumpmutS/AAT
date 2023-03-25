using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent
{
    public event Action OnPathSet;
    public event Action OnPathFinished;
    public event Action OnWarped; 

    public void Activate();
    public bool IsActive();
    public bool CalculateTeleportPath(Vector3 destination, out List<TeleportPoint> points);
    public void SetDestination(Vector3 destination);
    public void ClearDestination();
    public void EnableAgent(object caller);
    public void DisableAgent(object caller);
    public float GetSpeed();
    public void Warp(Vector3 position);
}