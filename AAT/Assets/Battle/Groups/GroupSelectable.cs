using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Group))]
public class GroupSelectable : Selectable
{
    private Group _group;
    private HashSet<Selectable> _memberSelectables = new();

    private void Awake()
    {
        _group = GetComponent<Group>();
        _group.OnMembersChanged += UpdateSelectables;
    }

    private void UpdateSelectables()
    {
        foreach (var member in _group.GroupMembers)
        {
            if (member.TryGetComponent<SelectionTarget>(out var target))
            {
                target.SetSelectable(this);
            }

            _memberSelectables.Clear();
            if (member.TryGetComponent<Selectable>(out var selectable))
            {
                _memberSelectables.Add(selectable);
            }
        }
    }

    protected override void Select()
    {
        base.Select();
        foreach (var selectable in _memberSelectables)
        {
            selectable.CallSelectOverrideUICheck();
        }
    }

    protected override void Deselect()
    {
        base.Deselect();
        foreach (var selectable in _memberSelectables)
        {
            selectable.CallDeselectOverrideUICheck();
        }
    }
}