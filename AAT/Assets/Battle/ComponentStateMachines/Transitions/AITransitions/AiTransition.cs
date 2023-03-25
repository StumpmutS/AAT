using UnityEditor;
using UnityEngine;

public abstract class AiTransition : Transition<AiTransitionBlackboard>, IStatePrefabGen
{
    [SerializeField] private AiComponentState fromPrefab;
    public override ComponentState<AiTransitionBlackboard> FromPrefab() => fromPrefab;
    [SerializeField] private AiComponentState toPrefab;
    public override ComponentState<AiTransitionBlackboard> ToPrefab() => toPrefab;

    [ContextMenu("GenerateStatePrefabs")]
    public void GenerateStatePrefabs()
    {
        if (!PrefabGen.TryGenerateStateFolder(GetInstanceID(), out var path))
        {
            Debug.LogWarning($"Prefab folder path already exists for {AssetDatabase.GetAssetPath(GetInstanceID())}. Did not update the folder");
            return;
        }
        
        if (!Any && PrefabGen.GenerateStatePrefab(fromState.TargetType, path, "From", out var fPrefab))
        {
            fromPrefab = fPrefab.GetComponent<AiComponentState>();
        }
        if (PrefabGen.GenerateStatePrefab(toState.TargetType, path, "To", out var tPrefab))
        {
            toPrefab = tPrefab.GetComponent<AiComponentState>();
        }
        Debug.Log($"Successfully created prefab folder for {AssetDatabase.GetAssetPath(GetInstanceID())}.");
    }
}