using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Factions/Faction Colors")]
public class FactionColors : ScriptableObject
{
    public SerializableDictionary<EFaction, Color> FactionsByColor = new SerializableDictionary<EFaction, Color>()
    {
        {EFaction.None, Color.clear},
        {EFaction.Neutral, Color.gray},
        {EFaction.Forest, Color.green},
        {EFaction.Monster, Color.black}
    };
}
