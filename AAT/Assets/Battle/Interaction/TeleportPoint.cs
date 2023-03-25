using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

[RequireComponent(typeof(GroupMember))]
public class TeleportPoint : InteractableController
{
    [SerializeField] private Vector3 exitPointOffset;
    [SerializeField] private float teleportTime;
    public float TeleportTime => teleportTime;

    public TeleportPoint OtherTeleportPoint { get; private set; }
    
    private bool _hoverSubscribed;
    
    public void SetupPair(TeleportPoint other)
    {
        OtherTeleportPoint = other;
        other.OtherTeleportPoint = this;
    }

    public override void RequestAffection(InteractionComponentState componentState)
    {
        StartCoroutine(WarpInteractor(componentState));
    }

    private IEnumerator WarpInteractor(InteractionComponentState componentState)
    {
        yield return new WaitForSeconds(teleportTime);
        if (componentState == null) yield break;
        
        componentState.Container.GetComponent<IMoveSystem>().Warp(new StumpTarget(null, OtherTeleportPoint.transform.position + OtherTeleportPoint.exitPointOffset));
        componentState.FinishInteraction();
    }
}