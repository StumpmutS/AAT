using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class DashController : MonoBehaviour
{
    private AgentBrain _agentBrain;
    private ComponentState<AgentTransitionBlackboard> _currentAgent;

    private List<Vector3> _dashPoints = new();
    private Vector3 _origPosition;
    private float _dashLerpSpeed;
    private float _currentBezierValue;

    protected virtual void Awake()
    {
        _agentBrain = GetComponent<AgentBrain>();
    }

    public virtual void Configure(IEnumerable<Vector3> points, float dashSpeed)
    {
        EndDash();
        _dashPoints = points.Select(p => p.x * transform.right + p.y * transform.up + p.z * transform.forward).ToList();
        _currentBezierValue = 0;
        _dashLerpSpeed = dashSpeed;
        _currentAgent = (ComponentState<AgentTransitionBlackboard>) _agentBrain.CurrentAgent;
        _origPosition = transform.position;
        _currentAgent.OnTick += Dash;
    }

    private void Dash()
    {
        _currentBezierValue += _dashLerpSpeed * _currentAgent.Runner.DeltaTime;
        transform.position = _origPosition + StumpBezierHelpers.SamplePoint(_dashPoints, _currentBezierValue);
        transform.forward = StumpBezierHelpers.SampleTangent(_dashPoints, _currentBezierValue);
        if (_currentBezierValue >= 1) EndDash();
    }

    public void EndDash()
    {
        if (_currentAgent != null) _currentAgent.OnTick -= Dash;
    }

    private void OnDestroy()
    {
        EndDash();
    }
}
