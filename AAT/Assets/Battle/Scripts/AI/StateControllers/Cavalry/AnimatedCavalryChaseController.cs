using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCavalryChaseController : BaseChaseController
{
    public float ChaseTime { get; private set; }

    protected bool _attacked;
    private bool _chaseStarted;
    protected GameObject _target;

    protected override void Chase(GameObject target)
    {
        base.Chase(target);
        if (!_chaseStarted)
        {
            _chaseStarted = true;
            _target = target;
            InputManager.OnUpdate += IncrementChase;
            DeactivateAI();
            _attacked = false;
        }
    }
    
    protected virtual void DeactivateAI()
    {
        AI.Deactivate();
    }

    protected override void StopChase()
    {
        base.StopChase();
        if (_chaseStarted)
        {
            _chaseStarted = false;
            InputManager.OnUpdate -= IncrementChase;
            ChaseTime = 0;
            ActivateAI();
        }
    }

    protected virtual void ActivateAI()
    {
        AI.Activate();
    }

    private void IncrementChase()
    {
        ChaseTime += Time.deltaTime;
        IncrementMovement();
    }

    protected virtual void IncrementMovement()
    {
        if (_target.GetComponent<UnitController>().IsDead && !_attacked)
        {
            StopChase();
        }
        agent.speed *= unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.ChaseSpeedPercentMultiplier] *= Time.deltaTime;
        if (Vector3.Distance(_target.transform.position, transform.position) < unitDataManager.CurrentUnitStatsData.UnitFloatStats[EUnitFloatStats.AttackRange])
        {
            _attacked = true;
            ActivateAI();
        }
    }
}
