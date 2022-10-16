using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

public static class ClassCollector
{
    public static Type[] GetTypesInheritingFrom(Type parent)
    {
        var appDomain = AppDomain.CurrentDomain;
        var assemblies = appDomain.GetAssemblies();
        return (from assembly in assemblies from type in assembly.GetTypes() where type.IsSubclassOf(parent) select type).ToArray();
    }

    public static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(Type fromType) where T : Attribute
    {
        return fromType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => f.GetCustomAttributes(typeof(T), true).Length > 0);
    }

    public static List<T> GetAssetsOfType<T>() where T : Object
    {
        List<T> assets = new();
        var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if(asset != null) assets.Add(asset);
        }
        return assets;
    }
}