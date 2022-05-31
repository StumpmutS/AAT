using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolComponentState : ComponentState
{
    private UnitController _unit;
    private AgentBrain _agentBrain;
    private IAgent _agent => _agentBrain.CurrentAgent;
    private List<Vector3> _patrolPoints = new();
    private int _currentPatrolPointIndex;

    private void Awake()
    {
        _unit = GetComponent<UnitController>();
        _agentBrain = GetComponent<AgentBrain>();
    }
    
    public override void OnEnter()
    {
        _agent.EnableAgent(this);
        _agent.OnPathFinished += NextPatrolPoint;
        _patrolPoints = _unit.PatrolPoints;
        SetDestination(0);
    }

    public override void OnExit()
    {
        _agent.OnPathFinished -= NextPatrolPoint;
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
        _agent.SetDestination(_patrolPoints[patrolPointIndex]);
        _currentPatrolPointIndex = patrolPointIndex;
    }

    public override bool Decision() => _unit.PatrolPoints.Count > 0;
}
