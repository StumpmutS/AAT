using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TypeKey, TypeValue> : Dictionary<TypeKey, TypeValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TypeKey> _keys = new List<TypeKey>();
    [SerializeField] private List<TypeValue> _values = new List<TypeValue>();

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        _keys = Keys.ToList();
        _values = Values.ToList();
    }

    public void OnAfterDeserialize()
    {
        Clear();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            Add(_keys[i], _values[i]); 
    }

    void OnGUI()
    {
        foreach (var kvp in this)
        {
            GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
        }
    }
}
