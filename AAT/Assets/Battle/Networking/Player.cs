using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Utility.Scripts;

[RequireComponent(typeof(TeamController))]
public class Player : NetworkBehaviour
{
    [Networked, Capacity(128)] public NetworkLinkedList<NetworkId> OwnedSectorIds => default;

    public static StumpTarget RightClickTarget { get; private set; }
    public static StumpTarget LeftClickTarget { get; private set; }
    public static int TeamNumber { get; private set; }

    private TeamController _team;

    public override void Spawned()
    {
        _team = GetComponent<TeamController>();
        TeamManager.Instance.SetPlayerForTeam(_team.GetTeamNumber(), Object.InputAuthority);
        TeamNumber = _team.GetTeamNumber();
    }

    public void AddSectors(IEnumerable<SectorController> sectors)
    {
        foreach (var sector in sectors)
        {
            OwnedSectorIds.Add(sector.Object.Id);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput<NetworkedInputData>(out var input) || input.RightClickPosition == default)
        {
            RightClickTarget = default;
            return;
        }

        LeftClickTarget = CollisionDetector.CheckHitPosition(Runner, Object.InputAuthority, input.LeftClickPosition, input.LeftClickDirection);
        RightClickTarget = CollisionDetector.CheckHitPosition(Runner, Object.InputAuthority, input.RightClickPosition, input.RightClickDirection);
    }
}

