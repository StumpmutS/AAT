using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Factions/Faction Hex Prefabs")]
public class FactionHexPrefabs : ScriptableObject
{
    public SerializableDictionary<EFaction, GameObjectListWrapper> FactionsByHexPrefab = new()
    {
        {EFaction.None, null},
        {EFaction.Neutral, null},
        {EFaction.Forest, null},
        {EFaction.Monster, null}
    };
}