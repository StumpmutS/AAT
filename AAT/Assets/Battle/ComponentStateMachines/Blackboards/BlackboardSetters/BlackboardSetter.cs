using Fusion;
using UnityEngine;

public abstract class BlackboardSetter<T> : NetworkBehaviour where T : TransitionBlackboard
{
    [SerializeField] private Brain<T> brain;

    protected T _blackboard => brain.GetBlackboard();
}