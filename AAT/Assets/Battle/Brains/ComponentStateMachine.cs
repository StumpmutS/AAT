using System;
using System.Collections.Generic;
using UnityEngine;

public class ComponentStateMachine<T> where T : TransitionBlackboard
{
    private T transitionBlackboard;

    public ComponentState<T> CurrentComponentState { get; private set; }
    private ComponentState<T> _defaultComponentState;
    private Dictionary<ComponentState<T>, List<TransitionData<T>>> _transitions = new();
    private List<TransitionData<T>> _anyTransitions = new();
    private List<TransitionData<T>> _currentTransitions = new();
    private readonly List<TransitionData<T>> _emptyList = new();

    public event Action OnUniversalTick = delegate { };

    public ComponentStateMachine(T transitionBlackboard)
    {
        this.transitionBlackboard = transitionBlackboard;
    }
    
    private void SetState(ComponentState<T> componentState, bool selfSet = false)
    {
        if (componentState == CurrentComponentState && !selfSet) return;

        if (CurrentComponentState != null) CurrentComponentState.OnExit();
        CurrentComponentState = componentState;

        _transitions.TryGetValue(componentState, out _currentTransitions);
        _currentTransitions ??= _emptyList;
        
        componentState.TryOnEnter();
    }

    public bool Exit(ComponentState<T> from, ComponentState<T> to = null)
    {
        if (from != CurrentComponentState) return false;
        
        SetState(to == null ? _defaultComponentState : to);
        return true;
    }

    public void Tick()
    {
        var transition = GetTransition(out var selfSet);
        if (transition != null)
            SetState(transition.To, selfSet);
        
        OnUniversalTick.Invoke();
        if (CurrentComponentState != null) CurrentComponentState.CallTick();
    }

    public void AddTransition(ComponentState<T> from, ComponentState<T> to, Func<T, bool> condition, bool isDefault)
    {
        if (!_transitions.ContainsKey(from))
            _transitions[from] = new List<TransitionData<T>>();
        
        _transitions[from].Add(new TransitionData<T>(to, condition));
        if (isDefault) SetDefault(to);
    }

    public void AddAnyTransition(ComponentState<T> to, Func<T, bool> condition, bool isDefault)
    {
        _anyTransitions.Add(new TransitionData<T>(to, condition));
        if (isDefault) SetDefault(to);
    }

    private void SetDefault(ComponentState<T> componentState)
    {
        _defaultComponentState = componentState;
        CurrentComponentState = componentState;
    }

    public void ActivateDefault() => SetState(_defaultComponentState, true);

    private TransitionData<T> GetTransition(out bool selfSet)
    {
        selfSet = false;
        if (CurrentComponentState.IsInterruptable)
            foreach (var transition in _anyTransitions)
                if (transition.MainCondition(transitionBlackboard))
                    return transition;

        selfSet = true;
        foreach (var transition in _currentTransitions)
            if (transition.MainCondition(transitionBlackboard))
                return transition;
        
        return null;
    }
}