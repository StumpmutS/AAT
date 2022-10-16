using System.Collections;
using UnityEngine;

public class GroupSpawnPointController : SpawnPointController
{
    public void SpawnGroup(Group groupPrefab, float spawnTime, SectorController sector)
    {
        StartCoroutine(CoSpawnGroup(groupPrefab, spawnTime, sector));
    }
    
    private IEnumerator CoSpawnGroup(Group groupPrefab, float spawnTime, SectorController sector)
    {
        InvokeOnBeginSpawn();
        yield return new WaitForSeconds(spawnTime);

        var group = Runner.Spawn(groupPrefab, transform.position, Quaternion.identity);
        group.Init(sector);
        
        InvokeOnFinishedSpawn();
    }
}