using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class Brain<T> : SimulationBehaviour, ISpawned, IBrain<T> where T : TransitionBlackboard
{
    protected abstract Transition<T> GetDefaultTransition();
    protected abstract List<Transition<T>> GetTransitions();

    public ComponentStateMachine<T> ComponentStateMachine { get; private set; }
    private NetworkComponentStateContainer<T> _container;
    private T _transitionBlackboard;
    protected abstract T InitBlackboard();
    public T GetBlackboard() => _transitionBlackboard;

    private void Awake()
    {
        _container = GetComponent<NetworkComponentStateContainer<T>>();
        _transitionBlackboard = InitBlackboard();
    }

    public virtual void Spawned()
    {
        ComponentStateMachine = new ComponentStateMachine<T>(_transitionBlackboard);
        AddTransitions();
    }

    protected virtual void Start()
    {
        ComponentStateMachine.ActivateDefault();
    }

    private void AddTransitions()
    {
        SetupTransition(GetDefaultTransition(), true);

        foreach (var transition in GetTransitions())
        {
            SetupTransition(transition);
        }
    }

    private void SetupTransition(Transition<T> transition, bool isDefault = false)
    {
        var to = _container.AddOrGetComponentState(transition.ToPrefab(), this, ComponentStateMachine);
        
        if (transition.Any)
        {
            AddAnyTransition(to, transition.Decision, isDefault);
            return;
        }
        
        var from = _container.AddOrGetComponentState(transition.FromPrefab(), this, ComponentStateMachine);
        
        AddTransition(from, to, transition.Decision, isDefault);
    }

    private void AddAnyTransition(ComponentState<T> to, Func<T, bool> decision, bool isDefault)
    {
        ComponentStateMachine.AddAnyTransition(to, decision, isDefault);
    }

    private void AddTransition(ComponentState<T> from, ComponentState<T> to, Func<T, bool> decision, bool isDefault)
    {
        ComponentStateMachine.AddTransition(from, to, decision, isDefault);
    }

    public override void FixedUpdateNetwork()
    {
        ComponentStateMachine.Tick();
    }

    public ComponentState<T> AddOrGetState(ComponentState<T> componentState)
    {
        var component = _container.AddOrGetComponentState(componentState, this, ComponentStateMachine);
        component.SetValuesFrom(componentState);
        return component;
    }
}