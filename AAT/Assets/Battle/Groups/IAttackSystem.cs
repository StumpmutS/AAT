using System;

public interface IAttackSystem
{
    public void CallAttack(StumpTarget target);
    public void CallAnimationAttack();
    public void CallAnimationCrit();
}