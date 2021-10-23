using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Unit Spawn Ability Component")]
public class UnitSpawnAbilityComponentData : AbilityComponent
{
    [SerializeField] private UnitController unitPrefab;
    [SerializeField] private int UnitsPerOffset;
    [SerializeField] private List<Vector3> spawnOffsets;

    public override void ActivateComponent(UnitController unit)
    {
        var objTransform = unit.transform;
        int spawnOffsetIndex = 0;
        for (int i = 0; i < spawnOffsets.Count; i++)
        {
            for (int j = 0; j < UnitsPerOffset; j++)
            {
                var instantiatedUnit = Instantiate(unitPrefab, objTransform.position, objTransform.rotation);
                unit.UnitGroup.AddUnit(instantiatedUnit);
                unit.SectorController.AddUnit(instantiatedUnit);
                instantiatedUnit.transform.localPosition += instantiatedUnit.transform.right * spawnOffsets[spawnOffsetIndex].x;
                instantiatedUnit.transform.localPosition += instantiatedUnit.transform.up * spawnOffsets[spawnOffsetIndex].y;
                instantiatedUnit.transform.localPosition += instantiatedUnit.transform.forward * spawnOffsets[spawnOffsetIndex].z;
            }
            spawnOffsetIndex++;
        }
    }
}
