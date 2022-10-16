using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utility.Scripts;

[CustomPropertyDrawer(typeof(TypeReference))]
public class TypeReferencePropertyDrawer : PropertyDrawer
{
    private class TypeReferencePropertyData
    {
        public bool FoldedOut;
    }
    
    private SerializedProperty _generatePrefab;
    private SerializedProperty _targetKeyWordProperty;
    private SerializedProperty _targetAssemblyQNameProperty;

    private static Dictionary<string, TypeReferencePropertyData> _perPropertyData = new();

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var propertyData = RefreshPropertyData(property);
        return base.GetPropertyHeight(property, label) * (propertyData.FoldedOut ? 3 : 1);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var propertyData = RefreshPropertyData(property);
        
        position.height /= 3;
        propertyData.FoldedOut = EditorGUI.Foldout(position, propertyData.FoldedOut, label);
        if (!propertyData.FoldedOut) return;
        
        EditorGUI.indentLevel++; //-------------------------------------------------------------------------------------
        
        position.y += position.height;
        _targetKeyWordProperty = property.FindPropertyRelative("searchKeyWord");
        var search = _targetKeyWordProperty.stringValue;
        search = EditorGUI.TextField(position, "Search Key Word", search);
        _targetKeyWordProperty.stringValue = search;

        position.y += position.height;
        var availableTypeTuples = ClassCollector.GetTypesInheritingFrom(typeof(MonoBehaviour))
            .Where(t => t != null)
            .Select(t => (t.Name, t.AssemblyQualifiedName));
        Dictionary<string, string> availableAssemblyQNames = new();
        foreach (var (name, assemblyQName) in availableTypeTuples)
        {
            if (!availableAssemblyQNames.ContainsKey(name))
            {
                availableAssemblyQNames.Add(name, assemblyQName);
            }
        }
        var availableTypeNames = availableAssemblyQNames.Keys.Where(s => s.ToLower().Contains(search.ToLower())).OrderBy(s => s).ToList();
        
        _targetAssemblyQNameProperty = property.FindPropertyRelative("targetTypeAssemblyQName");
        var type = Type.GetType(_targetAssemblyQNameProperty.stringValue);
        int selectedIndex = 0;
        if (type != null && availableTypeNames.Contains(type.Name))
        {
            selectedIndex = availableTypeNames.IndexOf(type.Name);
        }
        selectedIndex = EditorGUI.Popup(position, "Selected Type", selectedIndex, availableTypeNames.ToArray());
        _targetAssemblyQNameProperty.stringValue = availableAssemblyQNames[availableTypeNames[selectedIndex]];
        
        EditorGUI.indentLevel--;
    }

    private TypeReferencePropertyData RefreshPropertyData(SerializedProperty property)
    {
        if (!_perPropertyData.ContainsKey(property.propertyPath))
        {
            _perPropertyData[property.propertyPath] = new TypeReferencePropertyData();
        }
        return _perPropertyData[property.propertyPath];
    }
}