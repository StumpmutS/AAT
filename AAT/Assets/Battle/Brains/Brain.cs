using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class Brain<T> : SimulationBehaviour, ISpawned, IBrain<T> where T : TransitionBlackboard
{
    [SerializeField] private Transition<T> defaultTransition;
    [SerializeField] private List<Transition<T>> transitions;
    
    protected ComponentStateMachine<T> _componentStateMachine;
    private NetworkComponentStateContainer<T> _container;
    private T _transitionBlackboard;
    protected abstract T InitBlackboard();
    public T GetBlackboard() => _transitionBlackboard;

    private void Awake()
    {
        _container = GetComponent<NetworkComponentStateContainer<T>>();
        _transitionBlackboard = InitBlackboard();
    }

    public void Spawned()
    {
        _componentStateMachine = new ComponentStateMachine<T>(_transitionBlackboard);
        AddTransitions();
    }

    protected virtual void Start()
    {
        _componentStateMachine.ActivateDefault();
    }

    private void AddTransitions()
    {
        SetupTransition(defaultTransition, true);

        foreach (var transition in transitions)
        {
            SetupTransition(transition);
        }
    }

    private void SetupTransition(Transition<T> transition, bool isDefault = false)
    {
        var to = _container.AddOrGetComponentState(transition.ToPrefab, this, _componentStateMachine);
        to.SetValuesFrom(transition.ToPrefab);
        
        if (transition.Any)
        {
            AddAnyTransition(to, transition.Decision, isDefault);
            return;
        }
        
        var from = _container.AddOrGetComponentState(transition.FromPrefab, this, _componentStateMachine);
        from.SetValuesFrom(transition.FromPrefab);
        
        AddTransition(from, to, transition.Decision, isDefault);
    }

    private void AddAnyTransition(ComponentState<T> to, Func<T, bool> decision, bool isDefault)
    {
        _componentStateMachine.AddAnyTransition(to, decision, isDefault);
    }

    private void AddTransition(ComponentState<T> from, ComponentState<T> to, Func<T, bool> decision, bool isDefault)
    {
        _componentStateMachine.AddTransition(from, to, decision, isDefault);
    }

    public override void FixedUpdateNetwork()
    {
        _componentStateMachine.Tick();
    }

    public ComponentState<T> AddOrGetState(ComponentState<T> componentState)
    {
        var component = _container.AddOrGetComponentState(componentState, this, _componentStateMachine);
        component.SetValuesFrom(componentState);
        return component;
    }
}