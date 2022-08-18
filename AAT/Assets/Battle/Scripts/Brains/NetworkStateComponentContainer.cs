using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkStateComponentContainer : SimulationBehaviour
{
    private Dictionary<Type, ComponentState> _componentStates = new();

    public ComponentState AddOrGetComponentState(ComponentState state, ComponentStateMachine machine)
    {
        if (!Runner.IsServer) return null;
        
        var type = state.GetType();
        if (_componentStates.ContainsKey(type)) return _componentStates[type];
        
        var spawnedState = Runner.Spawn(state, transform.position, Quaternion.identity, Object.InputAuthority, InitState);
        _componentStates[type] = spawnedState;
        return spawnedState;

        void InitState(NetworkRunner _, NetworkObject o)
        {
            o.transform.parent = transform;
            o.GetComponent<ComponentState>().Init(machine, this);
        }
    }

    public bool TryGetComponentState(ComponentState state, out ComponentState foundState)
    {
        if (_componentStates.ContainsKey(state.GetType()))
        {
            foundState = _componentStates[state.GetType()];
            return true;
        }

        foundState = default;
        return false;
    }

    public bool TryGetComponentState(Type componentType, out ComponentState foundState)
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
