using UnityEngine;
using UnityEngine.Serialization;
using Utility.Scripts;

public abstract class Transition<T> : ScriptableObject where T : TransitionBlackboard
{
    [SerializeField] private bool any;
    public bool Any => any;
    
    [SerializeField] protected TypeReference fromState;
    public abstract ComponentState<T> FromPrefab();
    
    [SerializeField] protected TypeReference toState;
    public abstract ComponentState<T> ToPrefab();

    public abstract bool Decision(T transitionBlackboard);
}