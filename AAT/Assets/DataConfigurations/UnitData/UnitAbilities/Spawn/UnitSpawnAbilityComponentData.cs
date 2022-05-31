using System.Collections.Generic;
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
        var objTransform = unit.transform;

        if (!useCurrentGroup)
        {
            var unitGroup = Instantiate(unitGroupPrefab);
            SpawnUnits(unit, objTransform, unitGroup, 0);
        }
        
        for (int i = useCurrentGroup? 0 : 1; i < spawnOffsets.Count; i++)
        {
            SpawnUnits(unit, objTransform, unit.UnitGroup, i);
        }
    }
    
    private void SpawnUnits(UnitController unit, Transform objTransform, UnitGroupController unitGroup, int index)
    {
        for (int i = 0; i < unitsPerOffset; i++)
        {
            var instantiatedUnit = Instantiate(unitPrefab, objTransform.position, objTransform.rotation);
            unitGroup.AddUnit(instantiatedUnit);
            unit.Sector.AddUnit(instantiatedUnit);
            instantiatedUnit.transform.localPosition += objTransform.right * spawnOffsets[index].x
                                          + objTransform.up * spawnOffsets[index].y
                                          + objTransform.forward * spawnOffsets[index].z;
        }
    }
}
