using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPlotActionCreator : MonoBehaviour, IActionCreator
{
    [SerializeField] private SpawnPlotController spawnPlot;
    [SerializeField] private string category;
    [SerializeField] private List<SpawnerSpawnData> spawnerSpawnData;

    public List<UserAction> GetActions()
    {
        List<UserAction> userActions = new();
        foreach (var data in spawnerSpawnData.Where(d => spawnPlot.Faction == d.Faction))
        {
            userActions.Add(CreateAction(data));
        }

        return userActions;
    }

    private UserAction CreateAction(SpawnerSpawnData data)
    {
        return new UserAction(this, category, ESubCategory.Spawning, data.UserActionInfo.Label, data.UserActionInfo.Icon, 
            Spawn, HandleActionDeselection, data, data.UserActionInfo.KeyCode);
    }

    private void Spawn(object obj)
    {
        if (obj is not SpawnerSpawnData spawnData) return;
        
        spawnPlot.CallSetupSpawner(spawnData);
    }
    
    private void HandleActionDeselection(object _) { }
}