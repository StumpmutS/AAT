using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ComponentStatesRef
{
    [MenuItem("Utilities/RefreshStatePrefabs")]
    public static void RefreshStatePrefabs()
    {
        var assets = ClassCollector.GetAssetsOfType<Object>().Where(o => o.GetType().GetInterface(nameof(IStatePrefabGen)) != null);
        foreach (var asset in assets)
        {
            //asset.GetType().GetMethod("GenerateStatePrefabs").Invoke(asset, null);
            Debug.LogError($"Asset Name: {asset.name}, Asset Type: {asset.GetType().Name}, Asset Method: {asset.GetType().GetMethod("GenerateStatePrefabs").Name}");
        }
    }
}