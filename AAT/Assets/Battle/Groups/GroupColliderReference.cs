using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Group))]
public class GroupColliderReference : ColliderReference
{
    private Group _group;

    protected override void Awake()
    {
        base.Awake();
        _group = GetComponent<Group>();
        _group.OnMembersChanged += RefreshColliders;
    }

    protected override IColliderUpdater[] GetColliderUpdaters()
    {
        var colliderUpdaters = base.GetColliderUpdaters();
        colliderUpdaters.ToList().AddRange(_group.GroupMembers.SelectMany(m => m.GetComponentsInChildren<IColliderUpdater>()));
        return colliderUpdaters.ToArray();
    }
}