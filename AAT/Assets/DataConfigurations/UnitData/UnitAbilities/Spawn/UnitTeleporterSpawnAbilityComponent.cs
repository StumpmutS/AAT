using System.Collections;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Spawn/Unit Teleporter Spawn Ability Component")]
public class UnitTeleporterSpawnAbilityComponent : AbilityComponent
{
    [SerializeField] private UnitController teleportPointUnit;
    [SerializeField] private float timeForOtherPoint;
    [SerializeField] private Vector3 originOffset, destinationOffset;
    [SerializeField] private UnitGroupController unitGroupController;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        unit.StartCoroutine(Activate(unit, point));
    }

    private IEnumerator Activate(UnitController unit, Vector3 point)
    {
        var instantiatedUnitGroup = Instantiate(unitGroupController);
        var instantiatedOriginTeleporter = StumpNetworkRunner.Instance.Runner.Spawn(teleportPointUnit, unit.transform.position + originOffset, Quaternion.identity).GetComponent<TeleportPoint>();
        instantiatedUnitGroup.AddUnit(instantiatedOriginTeleporter.Unit);
        instantiatedOriginTeleporter.Unit.SetSector(unit.Sector);
        yield return new WaitForSeconds(timeForOtherPoint);
        //TODO: UNIT DEAD?
        
        var otherSector = SectorFinder.FindSector(point, 5, LayerManager.Instance.GroundLayer);
        var instantiatedDestinationTeleporter = StumpNetworkRunner.Instance.Runner.Spawn(teleportPointUnit, point + destinationOffset, Quaternion.identity).GetComponent<TeleportPoint>();
        instantiatedUnitGroup.AddUnit(instantiatedDestinationTeleporter.Unit);
        instantiatedDestinationTeleporter.Unit.SetSector(otherSector);
        instantiatedOriginTeleporter.SetupPair(unit.Sector, instantiatedDestinationTeleporter, otherSector);
        
        if (unit.TryGetComponent<MovementInteractOverrideComponentState>(out var state))
            state.SetDesiredInteractable(instantiatedOriginTeleporter);
        else
            Debug.LogWarning("This unit does not have an interaction state");

        var destinationRenderer = instantiatedDestinationTeleporter.GetComponentInChildren<Renderer>();
        destinationRenderer.enabled = false;
        yield return new WaitForSeconds(instantiatedDestinationTeleporter.TeleportTime);
        destinationRenderer.enabled = true;
    }
}
