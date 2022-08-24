using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ResourcePathGatherer : AssetPostprocessor
{
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        BuildResourceReference.StaticSOResourcePaths.Clear();
        
        DirectoryInfo assetDirectory = new DirectoryInfo(Application.dataPath + "\\Resources");
        var directories = assetDirectory.GetDirectories();
        if (directories.Length < 1)
        {
            Debug.LogError("Resources folder does not contain any further directories");
            return;
        }
        
        var firstResourceFolderName = assetDirectory.GetDirectories()[0].Name;
        var assets = Resources.LoadAll<ScriptableObject>(firstResourceFolderName);
        foreach (var asset in assets)
        {
            BuildResourceReference.StaticSOResourcePaths[asset] = GetResourcePath(asset);
        }
        if (assets.Length > 0) BuildResourceReference.CallRefresh();
    }
    
    private static string GetResourcePath(ScriptableObject so)
    {
        var assetPath = AssetDatabase.GetAssetPath(so); //remove ...Resources/ - .asset
        var noExtPath = assetPath.Remove(assetPath.IndexOf(".asset"), 6);
        return noExtPath.Remove(0, assetPath.IndexOf("Resources/") + 10);
    }
}
