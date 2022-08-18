using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Utility.Scripts;

[RequireComponent(typeof(UnitController))]
public class Brain : SimulationBehaviour, ISpawned
{
    [SerializeField] private NetworkStateComponentContainer container;
    public NetworkStateComponentContainer Container => container;
    [SerializeField] private Transition defaultTransition;
    [SerializeField] private List<Transition> transitions;

    protected ComponentStateMachine ComponentStateMachine;

    public void Spawned()
    {
        if (!Runner.IsServer) return;

        ComponentStateMachine = new ComponentStateMachine(GetComponent<UnitController>());
        AddTransitions();
    }

    protected virtual void Start()
    {
        if (!Runner.IsServer) return;
        
        ComponentStateMachine.ActivateDefault();
    }

    private void AddTransitions()
    {
        SetupTransition(defaultTransition, true);

        foreach (var transition in transitions)
        {
            SetupTransition(transition);
        }
    }

    private void SetupTransition(Transition transition, bool isDefault = false)
    {
        var to = container.AddOrGetComponentState(transition.To, ComponentStateMachine);
        to.SetValuesFrom(transition.To);
        
        if (transition.Any)
        {
            AddAnyTransition(to, transition.Decision, isDefault);
            return;
        }
        
        var from = container.AddOrGetComponentState(transition.From, ComponentStateMachine);
        from.SetValuesFrom(transition.From);
        
        AddTransition(from, to, transition.Decision, isDefault);
    }

    private void AddAnyTransition(ComponentState to, Func<UnitController, bool> decision, bool isDefault)
    {
        ComponentStateMachine.AddAnyTransition(to, decision, isDefault);
    }

    private void AddTransition(ComponentState from, ComponentState to, Func<UnitController, bool> decision, bool isDefault)
    {
        ComponentStateMachine.AddTransition(from, to, decision, isDefault);
    }

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer) return;
        
        ComponentStateMachine.Tick();
    }

    public ComponentState AddOrGetState(ComponentState componentState)
    {
        var component = container.AddOrGetComponentState(componentState, ComponentStateMachine);
        component.SetValuesFrom(componentState);
        return component;
    }
}
