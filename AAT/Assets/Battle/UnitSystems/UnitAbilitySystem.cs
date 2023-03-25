using System;
using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitAbilitySystem : NetworkBehaviour, IAbilitySystem
{
    [FormerlySerializedAs("container")] [SerializeField] private AiNetworkComponentStateContainer aiContainer;
    [SerializeField] private TransformContainer transformContainer;
    
    private SectorReference _sectorReference;
    private AbilityBrainComponentState _brainComponentState;

    private void Awake()
    {
        _sectorReference = GetComponent<SectorReference>();
    }

    public override void Spawned()
    {
        base.Spawned();
        if (aiContainer.TryGetComponentState(typeof(AbilityBrainComponentState), out var state))
        {
            _brainComponentState = (AbilityBrainComponentState) state;
        }
    }

    public void PrepareAbility(UnitAbilityData ability)
    {
    }

    public void UnPrepareAbility(UnitAbilityData ability)
    {
    }

    public void CastAbility(UnitAbilityData ability, StumpTarget target)
    {
        if (!RestrictionHelper.CheckRestrictions(ability.UnitAbilityDataInfo.Restrictions,
                new [] {new GameActionInfo(Object, _sectorReference.Sector, transformContainer.ToChain(), target)})) return;
        RpcCastAbility(BuildReferenceContainer.Instance.BuildResourceReference.SOResourcePaths[ability], TargetHelper.ConvertToNetworkedStumpTarget(target));
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcCastAbility(string abilityPath, NetworkedStumpTarget networkedTarget)
    {
        var ability = Resources.Load<UnitAbilityData>(abilityPath);
        var target = TargetHelper.ConvertToStumpTarget(networkedTarget);

        if (!RestrictionHelper.CheckRestrictions(ability.UnitAbilityDataInfo.Restrictions,
                new [] {new GameActionInfo(Object, _sectorReference.Sector, transformContainer.ToChain(), target)})) return;
        _brainComponentState.SetAbility(ability.UnitAbilityDataInfo.StatePrefab, target);
    }
}