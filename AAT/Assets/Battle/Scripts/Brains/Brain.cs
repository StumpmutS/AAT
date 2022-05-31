using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class Brain : MonoBehaviour
{
    [SerializeField] private Transition defaultTransition;
    [SerializeField] private List<Transition> transitions;

    protected ComponentStateMachine ComponentStateMachine;

    protected  virtual void Awake()
    {
        ComponentStateMachine = new ComponentStateMachine(GetComponent<UnitController>());
        AddTransitions();
    }

    protected virtual void Start() => ComponentStateMachine.ActivateDefault();

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
        if (!TryGetComponent(transition.To.GetType(), out var to))
            to = gameObject.AddComponent(transition.To.GetType());
        ((ComponentState) to).SetValuesFrom(transition.To);
        
        if (transition.Any)
        {
            AddAnyTransition(to as ComponentState, transition.Decision, isDefault);
            return;
        }
        
        if (!TryGetComponent(transition.From.GetType(), out var from))
            from = gameObject.AddComponent(transition.From.GetType());
        ((ComponentState) from).SetValuesFrom(transition.From);
        AddTransition(from as ComponentState, to as ComponentState, transition.Decision, isDefault);
    }

    private void AddAnyTransition(ComponentState to, Func<UnitController, bool> decision, bool isDefault)
    {
        ComponentStateMachine.AddAnyTransition(to, decision, isDefault);
        to.SetStateMachine(ComponentStateMachine);
    }

    private void AddTransition(ComponentState from, ComponentState to, Func<UnitController, bool> decision, bool isDefault)
    {
        ComponentStateMachine.AddTransition(from, to, decision, isDefault);
        from.SetStateMachine(ComponentStateMachine);
        to.SetStateMachine(ComponentStateMachine);
    }

    private void Update() => ComponentStateMachine.Tick();

    public ComponentState AddState(ComponentState componentState)
    {
        if (!TryGetComponent(componentState.GetType(), out var component))
        {
            component = gameObject.AddComponent(componentState.GetType());
            ((ComponentState) component).SetValuesFrom(componentState);
        }
        var returnState = (ComponentState) component;
        returnState.SetStateMachine(ComponentStateMachine);
        return returnState;
    }
}
