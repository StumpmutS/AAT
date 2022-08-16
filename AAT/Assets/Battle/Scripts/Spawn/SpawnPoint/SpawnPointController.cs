using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SpawnPointController : NetworkBehaviour
{
    [SerializeField] private TeamController team;
    
    public bool IsSpawning { get; private set; }
    
    public IEnumerator CoSpawnUnits(UnitController unitPrefab, UnitGroupController group, SectorController sector, float spawnTime, int amount, Action<List<UnitController>> unitsCallback)
    {
        if (!Runner.IsServer) yield break;
        
        yield return new WaitForSeconds(spawnTime);
        List<UnitController> units = new();
        for (var i = 0; i < amount; i++)
        {
            units.Add(Runner.Spawn(unitPrefab, transform.position, Quaternion.identity, Object.InputAuthority, InitUnit).GetComponent<UnitController>());
        }
        unitsCallback?.Invoke(units);

        void InitUnit(NetworkRunner _, NetworkObject o)
        {
            o.GetComponent<UnitController>().Init(team.GetTeamNumber(), sector, group);
        }
    }
}
