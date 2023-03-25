using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolComponentState : AiComponentState
{
    [SerializeField] private List<Vector3> startPatrolPoints;

    private IMoveSystem _moveSystem;
    private List<Vector3> _patrolPoints = new();
    private int _currentPatrolPointIndex;

    protected override void OnSpawnSuccess()
    {
        _patrolPoints = startPatrolPoints;
    }

    protected override void OnEnter()
    {
        _moveSystem.OnPathFinished += NextPatrolPoint;
        SetDestination(0);
    }

    protected override void Tick() { }

    public override void OnExit()
    {
        _moveSystem.OnPathFinished -= NextPatrolPoint;
    }

    private void NextPatrolPoint()
    {
        if (_currentPatrolPointIndex < _patrolPoints.Count - 1)
            SetDestination(_currentPatrolPointIndex + 1);
        else
            SetDestination(0);
    }

    private void SetDestination(int patrolPointIndex)
    {
        _moveSystem.SetTarget(new StumpTarget(null, _patrolPoints[patrolPointIndex]));
        _currentPatrolPointIndex = patrolPointIndex;
    }
}
