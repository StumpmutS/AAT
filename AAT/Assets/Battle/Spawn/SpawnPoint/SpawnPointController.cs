using System;
using Fusion;
using UnityEngine;

public class SpawnPointController<T> : NetworkBehaviour
{
    [SerializeField] protected TeamController team;
    
    public event Action<SpawnPointController<T>> OnBeginSpawn = delegate { };
    protected void InvokeOnBeginSpawn() => OnBeginSpawn.Invoke(this);
    public event Action<SpawnPointController<T>> OnCancelledSpawn = delegate { };
    public event Action<SpawnPointController<T>, T> OnFinishedSpawn = delegate { };
    protected void InvokeOnFinishedSpawn(T spawned) => OnFinishedSpawn.Invoke(this, spawned);

    public virtual void CancelSpawn()
    {
        OnCancelledSpawn.Invoke(this);
    }
}