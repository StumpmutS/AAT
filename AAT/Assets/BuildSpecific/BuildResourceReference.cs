using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Build Resource Ref")]
public class BuildResourceReference : ScriptableObject, ISerializationCallbackReceiver
{
    public static Dictionary<ScriptableObject, string> StaticSOResourcePaths = new();
    private static bool _refreshReady;
    public SerializableDictionary<ScriptableObject, string> SOResourcePaths;

    public static void CallRefresh() => _refreshReady = true;

    private void TryRefresh()
    {
        if (!_refreshReady || StaticSOResourcePaths.Count < 1 || StaticSOResourcePaths == null) return;
        
        UpdatePaths();
        _refreshReady = false;
        
        Debug.Log($"Successfully set all resource paths with a total of {StaticSOResourcePaths.Count} assets");
    }

    private void UpdatePaths()
    {
        SOResourcePaths.Clear();
        foreach (var kvp in StaticSOResourcePaths)
        {
            SOResourcePaths.Add(kvp.Key, kvp.Value);
        }
    }

    public void OnBeforeSerialize() => TryRefresh();

    public void OnAfterDeserialize() => TryRefresh();
}
