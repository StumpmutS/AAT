using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Spawn/Unit Teleporter Spawn Ability Component")]
public class UnitTeleporterSpawnAbilityComponent : AbilityComponent
{
    [SerializeField] private UnitController teleportPointUnit;
    [SerializeField] private float timeForOtherPoint;
    [SerializeField] private Vector3 originOffset, destinationOffset;
    [FormerlySerializedAs("unitGroupController")] [SerializeField] private UnitGroup unitGroup;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        unit.StartCoroutine(Activate(unit, point));
    }

    private IEnumerator Activate(UnitController unit, Vector3 point)
    {
        if (!unit.Runner.IsServer) yield break;
        
        var instantiatedOriginTeleporter = unit.Runner.Spawn(teleportPointUnit, unit.transform.position + originOffset, Quaternion.identity, unit.Object.InputAuthority,
            (_, o) =>
            {
                var teleportPoint = o.GetComponent<TeleportPoint>();
                TeamManager.Instance.SetupWithTeam(teleportPoint.Unit.Team, unit.Team.GetTeamNumber());
                teleportPoint.Unit.SetSector(unit.Sector);
            }).GetComponent<TeleportPoint>();
        yield return new WaitForSeconds(timeForOtherPoint);
        //TODO: UNIT DEAD?

        var otherSector = SectorFinder.FindSector(point, 5, LayerManager.Instance.GroundLayer);
        var instantiatedDestinationTeleporter = unit.Runner.Spawn(teleportPointUnit, point + destinationOffset, Quaternion.identity, unit.Object.InputAuthority,
            (_, o) =>
            {
                var teleportPoint = o.GetComponent<TeleportPoint>();
                TeamManager.Instance.SetupWithTeam(teleportPoint.Unit.Team, unit.Team.GetTeamNumber());
                teleportPoint.Unit.SetSector(otherSector);
                
            }).GetComponent<TeleportPoint>();
        instantiatedOriginTeleporter.SetupPair(unit.Sector, instantiatedDestinationTeleporter, otherSector);

        if (unit.TryGetComponent<InteractingController>(out var interactingController))
            interactingController.InteractWith(instantiatedOriginTeleporter);
        else
            Debug.LogWarning("This unit does not have an interacting controller");

        var destinationRenderer = instantiatedDestinationTeleporter.GetComponentInChildren<Renderer>();
        destinationRenderer.enabled = false;
        yield return new WaitForSeconds(instantiatedDestinationTeleporter.TeleportTime);
        destinationRenderer.enabled = true;
    }
}
