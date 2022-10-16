public class GroupAttackSystem : GroupSystem, IAttackSystem
{
    public void CallAttack(StumpTarget target)
    {
        foreach (var groupMember in group.GroupMembers)
        {
            groupMember.GetComponent<IAttackSystem>().CallAttack(target);
        }
    }
}