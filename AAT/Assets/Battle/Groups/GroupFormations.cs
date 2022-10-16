using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectDefaults/Group Formations")]
public class GroupFormations : ScriptableObject, ISerializationCallbackReceiver
{
    [Tooltip("Assume unit radius of 1")]
    [SerializeField] private SerializableDictionary<int, List<Vector3>> formations;

    public Dictionary<int, List<Vector3>> Formations = new();

    private void RefreshFormations()
    {
        Formations.Clear();
        
        foreach (var kvp in formations)
        {
            Formations[kvp.Key] = kvp.Value;
        }
    }

    public void OnBeforeSerialize()
    {
        RefreshFormations();
    }

    public void OnAfterDeserialize()
    {
        RefreshFormations();
    }
}