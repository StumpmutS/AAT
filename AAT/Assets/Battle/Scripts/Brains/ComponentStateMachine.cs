using System;
using System.Collections.Generic;
using UnityEngine;

public class ComponentStateMachine
{
    private UnitController _unit;

    public ComponentState CurrentComponentState { get; private set; }
    private ComponentState _defaultComponentState;
    private Dictionary<ComponentState, List<TempTransition>> _transitions = new();
    private List<TempTransition> _anyTransitions = new();
    private List<TempTransition> _currentTransitions = new();
    private List<TempTransition> _emptyList = new();

    public ComponentStateMachine(UnitController unit)
    {
        _unit = unit;
    }
    
    private void SetState(ComponentState componentState, bool selfSet = false)
    {
        if (componentState == CurrentComponentState && !selfSet) return;

        if (CurrentComponentState != null) CurrentComponentState.OnExit();
        CurrentComponentState = componentState;

        _transitions.TryGetValue(componentState, out _currentTransitions);
        _currentTransitions ??= _emptyList;
        
        componentState.TryOnEnter();
    }

    public void Exit(ComponentState from, ComponentState to = null)
    {
        if (from == CurrentComponentState) SetState(to == null ? _defaultComponentState : to);
    }

    public void Tick()
    {
        var transition = GetTransition(out var selfSet);
        if (transition != null)
            SetState(transition.To, selfSet);
        
        if (CurrentComponentState != null) CurrentComponentState.Tick();
    }

    public void AddTransition(ComponentState from, ComponentState to, Func<UnitController, bool> condition, bool isDefault)
    {
        if (!_transitions.ContainsKey(from))
            _transitions[from] = new List<TempTransition>();
        
        _transitions[from].Add(new TempTransition(to, condition));
        if (isDefault) SetDefault(to);
    }

    public void AddAnyTransition(ComponentState to, Func<UnitController, bool> condition, bool isDefault)
    {
        _anyTransitions.Add(new TempTransition(to, condition));
        if (isDefault) SetDefault(to);
    }

    private void SetDefault(ComponentState componentState)
    {
        _defaultComponentState = componentState;
        CurrentComponentState = componentState;
    }

    public void ActivateDefault() => SetState(_defaultComponentState, true);

    private class TempTransition
    {
        public Func<UnitController, bool> MainCondition { get; }
        public ComponentState To { get; }

        public TempTransition(ComponentState to, Func<UnitController, bool> mainCondition)
        {
            To = to;
            MainCondition = mainCondition;
        }
    }

    private TempTransition GetTransition(out bool selfSet)
    {
        selfSet = false;
        if (CurrentComponentState.IsInterruptable)
            foreach (var transition in _anyTransitions)
                if (transition.MainCondition(_unit) && transition.To.Decision())
                    return transition;

        selfSet = true;
        foreach (var transition in _currentTransitions)
            if (transition.MainCondition(_unit) && transition.To.Decision())
                return transition;
        
        return null;
    }
}
