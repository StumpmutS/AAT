using System;

public class GroupMoveSystem : GroupSystem, IMoveSystem
{
    public event Action OnPathFinished = delegate { };
    
    public void Move(StumpTarget target)
    {
        foreach (var groupMember in group.GroupMembers)
        {
            groupMember.GetComponent<IMoveSystem>().Move(target);
        }
    }
    
    public void Follow(StumpTarget target)
    {
        foreach (var groupMember in group.GroupMembers)
        {
            groupMember.GetComponent<IMoveSystem>().Follow(target);
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