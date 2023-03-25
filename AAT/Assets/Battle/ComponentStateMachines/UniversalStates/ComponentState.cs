using System;
using Fusion;
using UnityEngine;

public abstract class ComponentState<T> : NetworkBehaviour where T : TransitionBlackboard
{
    [Networked] private NetworkBehaviourId containerId { get; set; }
    [SerializeField] private NetworkComponentStateContainer<T> container;
    public NetworkComponentStateContainer<T> Container { get; private set; }

    [SerializeField] private bool isInterruptable;
    public bool IsInterruptable => isInterruptable;

    protected IBrain<T> _brain;
    protected ComponentStateMachine<T> _stateMachine;
    
    public event Action OnTick = delegate { };


    /*protected abstract void SetContainerId(NetworkBehaviourId id);
    protected abstract NetworkBehaviourId GetContainerId();*/
    
    public void Init(IBrain<T> brain, ComponentStateMachine<T> componentStateMachine, NetworkComponentStateContainer<T> stateContainer)
    {
        _brain = brain;
        _stateMachine = componentStateMachine;
        _stateMachine.OnUniversalTick += UniversalTick;
        Container = stateContainer;
        containerId = stateContainer.Id;
        //SetContainerId(stateContainer.Id);
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
            
            Container = behaviour.GetComponent<NetworkComponentStateContainer<T>>();
            transform.parent = Container.transform;
        }
        
        transform.parent = Container.transform;
        
        OnSpawnSuccess();
    }

    protected virtual void OnSpawnSuccess() { }

    public void TryOnEnter()
    {
        if (Object != null && !Runner.IsServer) return;
        OnEnter();
    }

    protected abstract void OnEnter();

    public void CallTick()
    {
        OnTick.Invoke();
        Tick();
    }

    protected abstract void Tick();
    
    public abstract void OnExit();
    
    protected virtual void UniversalTick() { }

    public virtual void SetValuesFrom(ComponentState<T> componentState)
    {
        isInterruptable = componentState.isInterruptable;
    }
}
