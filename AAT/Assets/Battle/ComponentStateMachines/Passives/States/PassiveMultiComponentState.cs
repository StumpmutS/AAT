using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

public class PassiveMultiComponentState : PassiveComponentState, IStatePrefabGen 
{
    [SerializeField] private List<TypeReference> types;
    [SerializeField, HideInInspector] private List<PassiveComponentState> states;

    private List<PassiveComponentState> _createdStates = new();

    protected override void OnSpawnSuccess()
    {
        foreach (var state in states)
        {
            _brain.AddOrGetState(state);
        }
    }

    protected override void OnEnter()
    {
        foreach (var state in _createdStates)
        {
            state.TryOnEnter();
        }
    }

    protected override void Tick()
    {
        foreach (var state in _createdStates)
        {
            state.CallTick();
        }
    }

    public override void OnExit()
    {
        foreach (var state in _createdStates)
        {
            state.OnExit();
        }
    }

    public void GenerateStatePrefabs()
    {
        if (!PrefabGen.TryGenerateStateFolder(GetInstanceID(), out var path)) return;

        List<PassiveComponentState> componentStates = new();

        for (var i = 0; i < types.Count; i++)
        {
            if (PrefabGen.GenerateStatePrefab(types[i].TargetType, path, $"Index{i}", out var prefab))
            {
                componentStates.Add(prefab.GetComponent<PassiveComponentState>());
            }
        }

        states = componentStates;
    }
}