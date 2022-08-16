using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (MeetsConditions(property))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!MeetsConditions(property)) return 0;
        return base.GetPropertyHeight(property, label);
    }

    private bool MeetsConditions(SerializedProperty property)
    {
        var showIfAttribute = (ShowIfAttribute)attribute;
        var target = property.serializedObject.targetObject;
        var condition = showIfAttribute.Condition;
        var conditionField = FindField(10, new HashSet<Object>() { target }, condition, out var newTarget);
        var conditionValue = conditionField.GetValue(newTarget);

        return (bool) conditionValue == showIfAttribute.Value;
    }

    private FieldInfo FindField(int maxRecursions, HashSet<Object> targets, string name, out Object newTarget)
    {
        newTarget = null;
        if (maxRecursions < 1) return null;
        
        var foundTargets = new HashSet<Object>();
        
        foreach (var target in targets)
        {
            var type = target.GetType();
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (field.Name == name)
                {
                    newTarget = target;
                    return field;
                }
                foundTargets.Add(field.GetValue(target));
            }
        }

        maxRecursions--;
        return FindField(maxRecursions, foundTargets, name, out newTarget);
    }
}
