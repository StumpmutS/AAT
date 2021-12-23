using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Spawn/Unit Teleporter Spawn Ability Component")]
public class UnitTeleporterSpawnAbilityComponent : AbilityComponent
{
    [SerializeField] private TeleportPoint teleportPoint;
    [SerializeField] private Vector3 originOffset, destinationOffset;
    [SerializeField] private UnitGroupController unitGroupController;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        var instantiatedUnitGroup = Instantiate(unitGroupController);
        var instaniatedOriginTeleporter = Instantiate(teleportPoint, unit.transform.position + originOffset, Quaternion.identity);
        var instantiatedDestinationTeleporter = Instantiate(teleportPoint, point + destinationOffset, Quaternion.identity);
        instaniatedOriginTeleporter.Setup(unit.SectorController, instantiatedDestinationTeleporter);
        instantiatedDestinationTeleporter.Setup(unit.SectorController, instaniatedOriginTeleporter);
        instaniatedOriginTeleporter.Unit.SetGroup(instantiatedUnitGroup);
        instaniatedOriginTeleporter.Unit.SetSector(unit.SectorController);
        instantiatedDestinationTeleporter.Unit.SetGroup(instantiatedUnitGroup);
        instantiatedDestinationTeleporter.Unit.SetSector(unit.SectorController);
    }
}