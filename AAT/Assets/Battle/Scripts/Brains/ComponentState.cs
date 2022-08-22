using System;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ComponentState : NetworkBehaviour
{
    [Networked] private NetworkBehaviourId containerId { get; set; }
    [SerializeField] private NetworkStateComponentContainer container;
    public NetworkStateComponentContainer Container { get; private set; }

    [FormerlySerializedAs("IsInterruptable")] [SerializeField] private bool isInterruptable;
    public bool IsInterruptable => isInterruptable;

    protected ComponentStateMachine _componentStateMachine;
    
    public event Action OnTick = delegate { };
    
    public void Init(ComponentStateMachine componentStateMachine, NetworkStateComponentContainer container)
    {
        _componentStateMachine = componentStateMachine;
        Container = container;
    }

    public override void Spawned()
    {
        if (Container == null)
        {
            if (container != null)
            {
                Container = container;
                return;
            }
            if (!Runner.TryFindBehaviour(containerId, out var behaviour)) return;
            
            Container = behaviour.GetComponent<NetworkStateComponentContainer>();
            transform.parent = Container.transform;
        }
        
        transform.parent = Container.transform;
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
        isInterruptable = componentState.isInterruptable;
    }
}
