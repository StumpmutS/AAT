using System;
using System.Collections.Generic;
using UnityEngine;

public class GroupMoveSystem : GroupSystem, IMoveSystem
{
    public event Action OnPathFinished = delegate { };
    public event Action<StumpTarget> OnNewTarget;

    public void SetTarget(StumpTarget target)
    {
        foreach (var groupMember in group.GroupMembers)
        {
            groupMember.GetComponent<IMoveSystem>().SetTarget(target);
        }
    }

    public void Warp(StumpTarget target)
    {
        foreach (var groupMember in group.GroupMembers)
        {
            groupMember.GetComponent<IMoveSystem>().Warp(target);
        }
    }

    public void Stop()
    {
        foreach (var groupMember in group.GroupMembers)
        {
            groupMember.GetComponent<IMoveSystem>().Stop();
        }
    }

    public void Enable()
    {
        foreach (var groupMember in group.GroupMembers)
        {
            groupMember.GetComponent<IMoveSystem>().Enable();
        }
    }

    public void Disable()
    {
        foreach (var groupMember in group.GroupMembers)
        {
            groupMember.GetComponent<IMoveSystem>().Disable();
        }
    }
}