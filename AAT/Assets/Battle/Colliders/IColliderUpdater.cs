using System;
using System.Collections.Generic;
using UnityEngine;

public interface IColliderUpdater
{
    public event Action OnCollidersChanged;
    public List<Collider> GetColliders();
}