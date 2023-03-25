using UnityEngine;

public abstract class AgentTransition : Transition<AgentTransitionBlackboard>, IStatePrefabGen
{
    [SerializeField] private AgentComponentState fromPrefab;
    public override ComponentState<AgentTransitionBlackboard> FromPrefab() => fromPrefab;
    [SerializeField] private AgentComponentState toPrefab;
    public override ComponentState<AgentTransitionBlackboard> ToPrefab() => toPrefab;
    
    [ContextMenu("GenerateStatePrefabs")]
    public void GenerateStatePrefabs()
    {
        if (!PrefabGen.TryGenerateStateFolder(GetInstanceID(), out var path)) return;
        
        if (!Any && PrefabGen.GenerateStatePrefab(fromState.TargetType, path, "From", out var fPrefab))
        {
            fromPrefab = fPrefab.GetComponent<AgentComponentState>();
        }
        if (PrefabGen.GenerateStatePrefab(toState.TargetType, path, "To", out var tPrefab))
        {
            toPrefab = tPrefab.GetComponent<AgentComponentState>();
        }
    }
}