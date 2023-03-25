using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

[RequireComponent(typeof(Group))]
public class GroupUpgradeManager : UpgradeManager
{
    private Group _group;
    
    protected override void Awake()
    {
        base.Awake();
        _group = GetComponent<Group>();
        _group.OnMembersChanged += HandleMembersChanged;
    }
    
    private void HandleMembersChanged()
    {
        foreach (var member in _group.GroupMembers)
        {
            if (member.TryGetComponent<UpgradeManager>(out var upgradeManager))
            {
                upgradeManager.Init(_upgrades, _upgradeIndexMap.ToList());
            }
        }
    }

    private void OnDestroy()
    {
        if (_group != null) _group.OnMembersChanged -= HandleMembersChanged;
    }

    public override IEnumerable<GameActionInfo> GetInfo()
    {
        return _group.GetCallingPoints().Select(tc => new GameActionInfo(Object, _group.Sector, tc,
            new StumpTarget(_group.gameObject, _group.transform.position)));
    }
}