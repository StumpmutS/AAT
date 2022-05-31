using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility.Scripts
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> _keys = new();
        [SerializeField] private List<TValue> _values = new();

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

            for (int i = 0; i != Mathf.Max(_keys.Count, _values.Count); i++)
            {
                if (_keys.Count <= i || _values.Count <= i)
                {
                    Add(default, default);
                }
                else
                {
                    Add(_keys[i], _values[i]);
                }
            }
        }
    }
}
