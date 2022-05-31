using System;
using UnityEngine;

public abstract class ComponentState : MonoBehaviour
{
    public bool IsInterruptable;

    protected ComponentStateMachine _componentStateMachine;
    public void SetStateMachine(ComponentStateMachine componentStateMachine) => _componentStateMachine = componentStateMachine;
    
    public event Action OnTick = delegate { };

    public virtual bool Decision() => true;
    public abstract void OnEnter();

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
