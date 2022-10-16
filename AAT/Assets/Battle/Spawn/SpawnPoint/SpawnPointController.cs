using System;
using Fusion;
using UnityEngine;

public class SpawnPointController : NetworkBehaviour
{
    [SerializeField] protected TeamController team;
    
    public event Action<SpawnPointController> OnBeginSpawn = delegate { };
    protected void InvokeOnBeginSpawn() => OnBeginSpawn.Invoke(this);
    public event Action<SpawnPointController> OnCancelledSpawn = delegate { };
    public event Action<SpawnPointController> OnFinishedSpawn = delegate { };
    protected void InvokeOnFinishedSpawn() => OnFinishedSpawn.Invoke(this);

    public virtual void CancelSpawn()
    {
        OnCancelledSpawn.Invoke(this);
    }
}