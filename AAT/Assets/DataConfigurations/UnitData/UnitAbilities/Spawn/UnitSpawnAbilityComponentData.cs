using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Spawn/Unit Spawn Ability Component")]
public class UnitSpawnAbilityComponentData : AbilityComponent
{
    [SerializeField] private bool useCurrentGroup;
    [SerializeField] private UnitGroupController unitGroupPrefab;
    [SerializeField] private UnitController unitPrefab;
    [SerializeField] private int unitsPerOffset;
    [SerializeField] protected List<Vector3> spawnOffsets;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        if (!unit.Runner.IsServer) return;

        if (!useCurrentGroup)
        {
            var unitGroup = unit.Runner.Spawn(unitGroupPrefab, inputAuthority:unit.Object.InputAuthority);
            SpawnUnits(unit, unitGroup, 0);
        }
        
        for (int i = useCurrentGroup? 0 : 1; i < spawnOffsets.Count; i++)
        {
            SpawnUnits(unit, unit.UnitGroup, i);
        }
    }
    
    private void SpawnUnits(UnitController unit, UnitGroupController unitGroup, int index)
    {
        for (int i = 0; i < unitsPerOffset; i++)
        {
            var instantiatedUnit = unit.Runner.Spawn(unitPrefab, unit.transform.position, unit.transform.rotation, unit.Object.InputAuthority, InitUnit);
            instantiatedUnit.transform.localPosition += unit.transform.right * spawnOffsets[index].x 
                                                        + unit.transform.up * spawnOffsets[index].y 
                                                        + unit.transform.forward * spawnOffsets[index].z;
        }

        void InitUnit(NetworkRunner _, NetworkObject o)
        {
            o.GetComponent<UnitController>().Init(unit.Team.GetTeamNumber(), unit.Sector, unitGroup);
        }
    }
}
