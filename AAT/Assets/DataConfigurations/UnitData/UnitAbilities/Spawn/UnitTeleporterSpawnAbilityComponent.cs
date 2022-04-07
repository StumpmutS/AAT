using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Spawn/Unit Teleporter Spawn Ability Component")]
public class UnitTeleporterSpawnAbilityComponent : AbilityComponent
{
    [SerializeField] private TeleportPoint teleportPoint;
    [SerializeField] private float timeForOtherPoint;
    [SerializeField] private Vector3 originOffset, destinationOffset;
    [SerializeField] private UnitGroupController unitGroupController;
    [SerializeField] private LayerMask ground;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        unit.StartCoroutine(Activate(unit, point));
    }

    private IEnumerator Activate(UnitController unit, Vector3 point)
    {
        var instantiatedUnitGroup = Instantiate(unitGroupController);
        var instantiatedOriginTeleporter = Instantiate(teleportPoint, unit.transform.position + originOffset, Quaternion.identity);
        instantiatedOriginTeleporter.Unit.SetGroup(instantiatedUnitGroup);
        instantiatedOriginTeleporter.Unit.SetSector(unit.SectorController);
        yield return new WaitForSeconds(timeForOtherPoint);
        var otherSector = SectorFinder.FindSector(point, 5, ground);
        var instantiatedDestinationTeleporter = Instantiate(teleportPoint, point + destinationOffset, Quaternion.identity);
        instantiatedDestinationTeleporter.Unit.SetGroup(instantiatedUnitGroup);
        instantiatedDestinationTeleporter.Unit.SetSector(unit.SectorController);
        instantiatedOriginTeleporter.SetupPair(unit.SectorController, instantiatedDestinationTeleporter, otherSector);
        instantiatedOriginTeleporter.SetupInteractions(new List<UnitController>() { unit });
        
        var destinationRenderer = instantiatedDestinationTeleporter.GetComponentInChildren<Renderer>();
        destinationRenderer.enabled = false;
        yield return new WaitForSeconds(instantiatedDestinationTeleporter.TeleportTime);
        destinationRenderer.enabled = true;
    }
}
