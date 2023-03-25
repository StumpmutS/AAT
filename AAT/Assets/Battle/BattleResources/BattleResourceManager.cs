using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BattleResourceManager : NetworkedSingleton<BattleResourceManager>
{
    public void AddResources(PlayerRef playerRef, EResourceType resourceType, int amount)
    {
        if (!Runner.IsServer) return;

        var player = GetPlayerFromPlayerRef(playerRef);
        player.Resources.Set(resourceType, player.Resources.Get(resourceType) + amount);
    }
    
    public void RemoveResources(PlayerRef playerRef, EResourceType resourceType, int amount)
    {
        if (!Runner.IsServer) return;
        
        var player = GetPlayerFromPlayerRef(playerRef);
        player.Resources.Set(resourceType, player.Resources.Get(resourceType) - amount);
    }

    public int GetResourceCount(PlayerRef playerRef, EResourceType resourceType)
    {
        return GetPlayerFromPlayerRef(playerRef).Resources.Get(resourceType);
    }

    private Player GetPlayerFromPlayerRef(PlayerRef playerRef)
    {
        return Runner.GetPlayerObject(playerRef).GetComponent<Player>();
    }
}

public enum EResourceType
{
    Gold
}