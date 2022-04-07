using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Factions/Faction Materials")]
public class FactionMaterials : ScriptableObject
{
    public SerializableDictionary<EFaction, Material> FactionsByMaterial =
        new SerializableDictionary<EFaction, Material>()
        {
            { EFaction.None, null },
            { EFaction.Neutral, null },
            { EFaction.Forest, null },
            { EFaction.Monster, null }
        };
}
