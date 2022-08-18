using System;
using Fusion;
using UnityEngine;

public abstract class ComponentState : NetworkBehaviour
{
    public bool IsInterruptable;

    protected ComponentStateMachine _componentStateMachine;
    public NetworkStateComponentContainer Container;
    
    public event Action OnTick = delegate { };
    
    public void Init(ComponentStateMachine componentStateMachine, NetworkStateComponentContainer container)
    {
        _componentStateMachine = componentStateMachine;
        Container = container;
    }

    public override void Spawned()
    {
        //TODO: network transform does not track parent, need custom implementation
        if (Container == null) Container = GetComponentInParent<NetworkStateComponentContainer>();
    }

    public virtual bool Decision() => true;

    public void TryOnEnter()
    {
        if (Object != null && !Runner.IsServer) return;
        OnEnter();
    }

    protected abstract void OnEnter();

    public virtual void Tick()
    {
        OnTick.Invoke();
    }
    
    public abstract void OnExit();

    public virtual void SetValuesFrom(ComponentState componentState)
    {
        IsInterruptable = componentState.IsInterruptable;
    }
}
