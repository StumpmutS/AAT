using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Factions/Faction Materials")]
public class FactionMaterials : ScriptableObject
{
    public SerializableDictionary<EFaction, Material> FactionsByMaterial =
        new()
        {
            { EFaction.None, null },
            { EFaction.Neutral, null },
            { EFaction.Forest, null },
            { EFaction.Monster, null }
        };
}
