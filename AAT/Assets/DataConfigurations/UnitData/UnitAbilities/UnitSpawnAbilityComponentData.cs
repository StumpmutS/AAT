using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Unit Spawn Ability Component")]
public class UnitSpawnAbilityComponentData : AbilityComponent
{
    [SerializeField] private UnitController unitPrefab;
    [SerializeField] private int UnitsPerOffset;
    [SerializeField] private List<Vector3> spawnOffsets;

    public override void ActivateComponent(GameObject gameObject)
    {
        var objTransform = gameObject.transform;
        int spawnOffsetIndex = 0;
        for (int i = 0; i < spawnOffsets.Count; i++)
        {
            for (int j = 0; j < UnitsPerOffset; j++)
            {
                var instantiatedUnit = Instantiate(unitPrefab, objTransform.position, objTransform.rotation);
                gameObject.GetComponent<UnitController>().UnitGroup.AddUnit(instantiatedUnit);
                instantiatedUnit.transform.localPosition += spawnOffsets[spawnOffsetIndex];
            }
            spawnOffsetIndex++;
        }
    }
}
