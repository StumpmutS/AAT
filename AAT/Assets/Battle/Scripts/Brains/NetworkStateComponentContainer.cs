using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkStateComponentContainer : NetworkBehaviour
{
    [Networked, Capacity(32)] NetworkLinkedList<NetworkBehaviourId> nStates => default;
    
    private Dictionary<Type, ComponentState> _componentStates = new();

    public ComponentState AddOrGetComponentState(ComponentState state, ComponentStateMachine machine)
    {
        var type = state.GetType();
        
        if (!Runner.IsServer)
        {
            foreach (var nState in nStates)
            {
                if (Runner.TryFindBehaviour(nState, out var behaviour) && behaviour.GetType() == type)
                {
                    var foundState = (ComponentState) behaviour.GetComponent(type);
                    _componentStates[type] = foundState;
                    foundState.Init(machine, this);
                    return foundState;
                }
            }
            
            Debug.LogError("State has not been spawned or does not exist");
            return null;
        }
        
        if (_componentStates.ContainsKey(type)) return _componentStates[type];
        var childState = GetComponentInChildren(type);
        if (childState != null)
        {
            _componentStates[type] = (ComponentState) childState;
            _componentStates[type].Init(machine, this);
            nStates.Add(_componentStates[type].Id);
            return _componentStates[type];
        }
        
        var spawnedState = Runner.Spawn(state, transform.position, Quaternion.identity, Object.InputAuthority, InitState);
        _componentStates[type] = spawnedState;
        nStates.Add(spawnedState.Id);
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
