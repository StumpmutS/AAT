using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UnitSpawnPointController : SpawnPointController
{
    public void SpawnUnits(UnitController unitPrefab, Group group, SectorController sector, float spawnTime, int amount, Action<List<UnitController>> unitsCallback)
    {
        StartCoroutine(CoSpawnUnits(unitPrefab, group, sector, spawnTime, amount, unitsCallback));
    }
    
    private IEnumerator CoSpawnUnits(UnitController unitPrefab, Group group, SectorController sector, float spawnTime, int amount, Action<List<UnitController>> unitsCallback)
    {
        if (!Runner.IsServer) yield break;

        InvokeOnBeginSpawn();
        yield return new WaitForSeconds(spawnTime);
        
        List<UnitController> units = new();
        for (var i = 0; i < amount; i++)
        {
            units.Add(Runner.Spawn(unitPrefab, transform.position, Quaternion.identity, Object.InputAuthority, InitUnit).GetComponent<UnitController>());
        }
        unitsCallback?.Invoke(units);
        
        InvokeOnFinishedSpawn();

        void InitUnit(NetworkRunner _, NetworkObject o)
        {
            o.GetComponent<UnitController>().Init(team.GetTeamNumber(), sector);
        }
    }
}