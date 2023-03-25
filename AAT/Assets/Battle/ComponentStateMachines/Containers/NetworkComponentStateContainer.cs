using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkComponentStateContainer<T> : NetworkBehaviour where T : TransitionBlackboard
{
    [Networked, Capacity(32)] NetworkLinkedList<NetworkBehaviourId> networkedStates => default;
    
    private Dictionary<Type, ComponentState<T>> _componentStates = new();

    public ComponentState<T> AddOrGetComponentState(ComponentState<T> state, IBrain<T> brain, ComponentStateMachine<T> machine)
    {
        var type = state.GetType();
        
        if (!Runner.IsServer)
        {
            foreach (var nState in networkedStates)
            {
                if (Runner.TryFindBehaviour(nState, out var behaviour) && behaviour.GetType() == type)
                {
                    var foundState = (ComponentState<T>) behaviour.GetComponent(type);
                    _componentStates[type] = foundState;
                    return foundState;
                }
            }
            
            Debug.LogError("State has not been spawned or does not exist");
            return null;
        }
        
        if (_componentStates.ContainsKey(type)) return _componentStates[type];
        
        var childStates = GetComponentsInChildren(type);
        foreach (var childState in childStates)
        {
            if (type != childState.GetType()) continue;
            _componentStates[type] = (ComponentState<T>) childState;
            networkedStates.Add(_componentStates[type].Id);
            return _componentStates[type];
        }

        var spawnedState = Runner.Spawn(state, transform.position, Quaternion.identity, Object.InputAuthority, InitState);
        _componentStates[type] = spawnedState;
        networkedStates.Add(spawnedState.Id);
        return spawnedState;

        void InitState(NetworkRunner r, NetworkObject o)
        {
            o.transform.parent = transform;
            var componentState = o.GetComponent<ComponentState<T>>();
            componentState.Init(brain, machine, this);
            componentState.SetValuesFrom(state);
        }
    }

    public bool TryGetComponentState(ComponentState<T> state, out ComponentState<T> foundState)
    {
        if (_componentStates.ContainsKey(state.GetType()))
        {
            foundState = _componentStates[state.GetType()];
            return true;
        }

        foundState = default;
        return false;
    }

    public bool TryGetComponentState(Type componentType, out ComponentState<T> foundState)
    {
        if (_componentStates.ContainsKey(componentType))
        {
            foundState = _componentStates[componentType];
            return true;
        }

        foundState = default;
        return false;
    }
}