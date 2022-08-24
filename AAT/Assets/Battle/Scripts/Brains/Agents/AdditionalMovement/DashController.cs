using Fusion;
using UnityEngine;

public class DashController : SimulationBehaviour
{
    private AgentBrain _agentBrain;
    private ComponentState _currentAgent;
    
    private float _dashTargetDistance;
    private float _dashSpeed;
    private float _distanceDashed;

    protected virtual void Awake()
    {
        _agentBrain = GetComponent<AgentBrain>();
    }

    public virtual void Dash(Vector3 distance, float speed)
    {
        _dashTargetDistance = distance.magnitude;
        _dashSpeed = speed;
        Reset();
        _currentAgent = (ComponentState) _agentBrain.CurrentAgent;
        _currentAgent.OnTick += PerformDash;
    }

    private void Reset()
    {
        if (_currentAgent != null) _currentAgent.OnTick -= PerformDash;
        _distanceDashed = 0;
    }

    private void PerformDash()
    {
        var newPos = Vector3.MoveTowards(transform.position, transform.position + transform.forward * _dashSpeed,
            _dashSpeed * Runner.DeltaTime);
        _distanceDashed += Vector3.Distance(transform.position, newPos);
        if (_distanceDashed >= _dashTargetDistance)
        {
            transform.position += (_distanceDashed - _dashTargetDistance) * transform.forward;
            Reset();
            return;
        }
        
        SetPosition(newPos);
    }

    protected virtual void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
