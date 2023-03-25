using System.Collections.Generic;

public class GroupHoverable : Hoverable
{
    private Group _group;
    private HashSet<Hoverable> _memberHoverables = new();

    private void Awake()
    {
        _group = GetComponent<Group>();
        _group.OnMembersChanged += UpdateHoverables;
    }

    private void UpdateHoverables()
    {
        foreach (var member in _group.GroupMembers)
        {
            if (member.TryGetComponent<SelectionTarget>(out var target))
            {
                target.SetHoverable(this);
            }

            _memberHoverables.Clear();
            if (member.TryGetComponent<Hoverable>(out var hoverable))
            {
                _memberHoverables.Add(hoverable);
            }
        }
    }

    public override void Hover()
    {
        base.Hover();
        foreach (var hoverable in _memberHoverables)
        {
            hoverable.Hover();
        }
    }

    public override void StopHover()
    {
        base.StopHover();
        foreach (var hoverable in _memberHoverables)
        {
            hoverable.StopHover();
        }
    }
}