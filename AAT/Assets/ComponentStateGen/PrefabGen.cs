using System;
using Fusion;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class PrefabGen
{
    public static bool TryGeneratePrefab(Type type, string assetPath, out GameObject prefab)
    {
        prefab = null;
        if (type.IsAbstract) return false;
        var tempGo = new GameObject();
        tempGo.AddComponent(type);
        prefab = PrefabUtility.SaveAsPrefabAsset(tempGo, assetPath, out var success);
        Object.DestroyImmediate(tempGo);
        return success;
    }
    
    public static bool TryGeneratePrefab<T>(string assetPath, out GameObject prefab)
    {
        var type = typeof(T);
        prefab = null;
        if (type.IsAbstract) return false;
        var tempGo = new GameObject();
        tempGo.AddComponent(type);
        prefab = PrefabUtility.SaveAsPrefabAsset(tempGo, assetPath, out var success);
        Object.DestroyImmediate(tempGo);
        return success;
    }
    
    public static bool GenerateStatePrefab(Type type, string folderPath, string prefix, out GameObject prefab)
    {
        var prefabPath = $"{folderPath}/{prefix}{type.Name}Prefab.prefab";
        if (!TryGeneratePrefab<NetworkObject>(prefabPath, out prefab)) return false;
        prefab.AddComponent(type);
        return true;
    }

    public static bool TryGenerateStateFolder(int instanceID, out string generatedPath)
    {
        var path = AssetDatabase.GetAssetPath(instanceID);
        path = path.Substring(0, path.LastIndexOf("."));
        path = path.Insert(path.Length, "States");
        var folderPath = path.Remove(path.LastIndexOf("/"), path.Length - path.LastIndexOf("/"));
        var folderName = path[(path.LastIndexOf("/") + 1)..];
        generatedPath = $"{folderPath}/{folderName}";
        if (AssetDatabase.IsValidFolder(generatedPath)) return false;
        AssetDatabase.CreateFolder(folderPath, folderName);
        return true;
    }
}