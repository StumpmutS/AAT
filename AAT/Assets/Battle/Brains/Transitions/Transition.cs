using UnityEngine;
using UnityEngine.Serialization;
using Utility.Scripts;

public abstract class Transition<T> : ScriptableObject, IStatePrefabGen where T : TransitionBlackboard
{
    [FormerlySerializedAs("Any")] [SerializeField] private bool any;
    public bool Any => any;
    
    [SerializeField] private TypeReference fromState;
    public ComponentState<T> FromPrefab { get; private set; }
    
    [SerializeField] private TypeReference toState;
    public ComponentState<T> ToPrefab { get; private set; }

    public abstract bool Decision(T transitionBlackboard);

    [ContextMenu("GenerateStatePrefabs")]
    public void GenerateStatePrefabs()
    {
        if (!PrefabGen.TryGenerateStateFolder(GetInstanceID(), out var path)) return;
        
        if (PrefabGen.GenerateStatePrefab(fromState.TargetType, path, "From", out var fPrefab))
        {
            FromPrefab = fPrefab.GetComponent<ComponentState<T>>();
        }
        if (PrefabGen.GenerateStatePrefab(toState.TargetType, path, "To", out var tPrefab))
        {
            ToPrefab = tPrefab.GetComponent<ComponentState<T>>();
        }
    }
}

public class AbilityReadyTransition : Transition<AiTransitionBlackboard>
{
    public override bool Decision(AiTransitionBlackboard transitionBlackboard)
    {
        return transitionBlackboard.AbilityReady;
    }
}