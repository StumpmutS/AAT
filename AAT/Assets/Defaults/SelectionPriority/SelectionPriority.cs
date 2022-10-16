using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectDefaults/Selection Priority")]
public class SelectionPriority : ScriptableObject
{
    [Tooltip("Highest prio starts at 0")]
    [SerializeField] private SerializableDictionary<ESelectionType, int> selectionPriority;

    public ESelectionType GetHighestPriority(IEnumerable<ESelectionType> selectionTypes)
    {
        return selectionTypes.OrderBy(s => selectionPriority[s]).FirstOrDefault();
    }
}
