using UnityEngine;public class UnitAttackSystem : MonoBehaviour, IAttackSystem
{
    /*protected virtual void CheckCrit()
    {
        if (Random.Range(0f, 100f) <= _critChancePercent)
        {
            AnimateCrit();
        }
        else
        {
            AnimateAttack();
        }
        StartCoroutine(StartAttackTimer());
    }

    protected void AnimateAttack()
    {
        _animTarget = _target;
        _attack = !_attack;
    }

    protected void AnimateCrit()
    {
        _animTarget = _target;
        _crit = !_crit;
    }

    public void AnimationTriggeredAttack()
    {
        if (_animTarget == null) return;
        var damage = -Mathf.Abs(_damage);
        _animTarget.Hit.GetComponent<IHealth>().ModifyHealth(damage);
        OnAttack.Invoke(_animTarget.Hit, damage);
    }

    public void AnimationTriggeredCrit()
    {
        if (_animTarget == null) return;
        var damage = -Mathf.Abs(_damage) * (_critMultiplierPercent / 100);
        _animTarget.Hit.GetComponent<IHealth>().ModifyHealth(damage);
        OnCrit.Invoke(_animTarget.Hit, damage);
    }*/

    public void CallAttack(StumpTarget target)
    {
        throw new System.NotImplementedException();
    }
}