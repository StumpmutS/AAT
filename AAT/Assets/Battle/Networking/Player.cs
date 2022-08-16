using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Networked, Capacity(128)] public NetworkLinkedList<NetworkId> OwnedSectorIds => default;

    public LagCompensatedHit NetworkInputHit { get; private set; }

    public override void Spawned()
    {
        TeamManager.Instance.SetPlayerForTeam(GetComponent<TeamController>().GetTeamNumber(), Object.InputAuthority);
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
            NetworkInputHit = default;
            return;
        }
        
        CheckHitPosition(input);
    }

    private void CheckHitPosition(NetworkedInputData input)
    {
        if (Runner.LagCompensation.Raycast(input.RightClickPosition - input.RightClickDirection.normalized * 50,
            input.RightClickDirection, 50.1f, Object.InputAuthority, out var hit))
        {
            NetworkInputHit = hit;
            return;
        }
        
        //Check for ground hit if no network hitboxes hit
        RaycastHit localHit;
        if (!Physics.Raycast(input.RightClickPosition - input.RightClickDirection.normalized * 50,
            input.RightClickDirection, out localHit, 50.1f, LayerManager.Instance.GroundLayer))
        {
            //check down from point if nothing found on direction
            Physics.Raycast(input.RightClickPosition, Vector3.down,
                out localHit, input.RightClickPosition.y + 1, LayerManager.Instance.GroundLayer);
        }

        hit.Collider = localHit.collider;
        hit.Point = localHit.point;
        hit.GameObject = localHit.collider.gameObject;
        NetworkInputHit = hit;
    }
}
