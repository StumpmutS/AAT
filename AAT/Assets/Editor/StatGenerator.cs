using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utility.Scripts;

public class StatGenerator : EditorWindow
{
    [SerializeField] private List<EUnitFloatStats> _stats = new()
    {
        EUnitFloatStats.MaxHealth,
        EUnitFloatStats.BaseArmorPercent,
        EUnitFloatStats.MaxArmorPercent,
        EUnitFloatStats.Damage,
        EUnitFloatStats.CritMultiplierPercent,
        EUnitFloatStats.CritChancePercent,
        EUnitFloatStats.AttackSpeedPercent,
        EUnitFloatStats.MovementSpeed,
        EUnitFloatStats.SightRange,
        EUnitFloatStats.AttackRange,
        EUnitFloatStats.ChaseSpeedPercentMultiplier
    };
    [SerializeField] private List<float> _values = new();
    [SerializeField] private int _amount = 11;

    [SerializeField] private string _unitName;
    [SerializeField] private string _statPurposeSuffix;
    
    private bool _generate;
    
    [MenuItem("Window/Stat Generator")]
    public static void OpenWindow()
    {
        var window = GetWindow<StatGenerator>();
        window.titleContent = new GUIContent("Stat Generator");
    }

    private void OnGUI()
    {
        _stats.Equalize(_amount);
        _values.Equalize(_amount);
        
        DrawUI();

        GUI.changed = true;
        if (GUI.changed) OnChanged();
    }

    private void OnChanged()
    {
        Repaint();
        if (_generate) GenerateObjects();
    }

    private void DrawUI()
    {
        var infoField = new Rect(10, position.height - 90, position.width / 2 - 15, 80);
        GUILayout.BeginArea(infoField);
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Suffixes:");
        EditorGUILayout.LabelField("B: Base, MS: Self, MO: Other, Un: Upgraden, An: Abilityn");
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
            
        var statField = new Rect(10, 40, position.width / 2 - 15, position.height - 40);
        GUILayout.BeginArea(statField);
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < _amount; i++)
        {
            _stats[i] = (EUnitFloatStats)EditorGUILayout.EnumPopup(_stats[i]);
        }
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
        
        var valueField = new Rect(position.width / 2 + 5, 40, position.width / 2 - 10, position.height - 40);
        GUILayout.BeginArea(valueField);
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < _amount; i++)
        {
            _values[i] = EditorGUILayout.FloatField(_values[i]);
        }
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
        
        var amountField = new Rect(10, 10, position.width - 10, 20);
        GUILayout.BeginArea(amountField);
        _amount = EditorGUILayout.DelayedIntField("Amount", _amount);
        GUILayout.EndArea();

        var generateField = new Rect(position.width / 2 + 5, position.height - 30, position.width / 2 - 10, 20);
        GUILayout.BeginArea(generateField);
        _generate = GUILayout.Button("Generate");
        GUILayout.EndArea();
        
        var suffixField = new Rect(position.width / 2 + 5, position.height - 60, position.width / 2 - 10, 20);
        GUILayout.BeginArea(suffixField);
        _statPurposeSuffix = EditorGUILayout.TextField("Stat Purpose Suffix", _statPurposeSuffix);
        GUILayout.EndArea();
        
        var nameField = new Rect(position.width / 2 + 5, position.height - 90, position.width / 2 - 10, 20);
        GUILayout.BeginArea(nameField);
        _unitName = EditorGUILayout.TextField("Unit Name", _unitName);
        GUILayout.EndArea();
    }

    private void GenerateObjects()
    {
        Debug.Log("GENERATE!");
        if (_stats.Count != _values.Count)
        {
            Debug.LogError("stats count does not equal values count");
            return;
        }

        for (int i = 0; i < _stats.Count; i++)
        {
            var generated = ScriptableObject.CreateInstance<UnitStat>();
            generated.Stat = _stats[i];
            generated.Value = _values[i];
            AssetDatabase.CreateAsset(generated, $"{AssetDatabase.GetAssetPath(Selection.activeObject)}/{_unitName}{_stats[i].ToString()}{_statPurposeSuffix}.asset");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
