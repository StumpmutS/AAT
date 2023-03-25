using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Passives/Sector Power Stats Passive Component")]
public class SectorPowerStatsBuffPassiveGameAction : SectorPowerDeterminedPassiveGameAction
{
    [SerializeField] private List<StatModifier> unitStatMods;

    private EffectContainer _effectContainer;

    protected override void OnSpawnSuccess()
    {
        base.OnSpawnSuccess();
        _effectContainer = Container.GetComponent<EffectContainer>();
    }

    protected override void ActivateThresholdIndex(SectorController sector, int index)
    {
        _effectContainer.AddEffect(unitStatMods[index]);
    }

    protected override void DeactivateThresholdIndex(SectorController sector, int index)
    {
        _effectContainer.RemoveEffect(unitStatMods[index]);
    }
}
