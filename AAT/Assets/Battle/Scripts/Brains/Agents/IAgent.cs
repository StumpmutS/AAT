using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent
{
    public event Action OnPathFinished;

    public void Activate();
    public bool IsActive();
    public bool CalculateTeleportPath(Vector3 destination, out List<TeleportPoint> points, out Vector3 finalDestination);
    public void SetDestination(Vector3 destination);
    public Vector3 GetDestination();
    public void ClearDestination();
    public void EnableAgent(object caller);
    public void DisableAgent(object caller);
    public void SetSpeedMultiplier(float speedMultiplier);
    public float GetSpeed();
    public void Warp(Vector3 position);
}