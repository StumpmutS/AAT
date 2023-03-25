using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(SectorReference))]
public class GroupAbilitySystem : GroupSystem, IAbilitySystem
{
    private SectorReference _sectorReference;
    
    private void Awake()
    {
        _sectorReference = GetComponent<SectorReference>();
    }

    public void PrepareAbility(UnitAbilityData ability)
    {
        foreach (var member in group.GroupMembers)
        {
            member.GetComponent<IAbilitySystem>().PrepareAbility(ability);
        }
    }

    public void UnPrepareAbility(UnitAbilityData ability)
    {
        foreach (var member in group.GroupMembers)
        {
            member.GetComponent<IAbilitySystem>().UnPrepareAbility(ability);
        }
    }

    public void CastAbility(UnitAbilityData ability, StumpTarget target)
    {
        if (!RestrictionHelper.CheckRestrictions(ability.UnitAbilityDataInfo.Restrictions, 
                group.GetCallingPoints().Select(tc => new GameActionInfo(Object, _sectorReference.Sector, tc, target)))) return;

        foreach (var member in group.GroupMembers)
        {
            member.GetComponent<IAbilitySystem>().CastAbility(ability, target);
        }
    }
}