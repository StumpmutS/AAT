using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TraverseController : MonoBehaviour
{
    [SerializeField] private AIPathfinder AI;
    [SerializeField] private UnitStatsModifierManager unitStatsModifier;
    [SerializeField] private LayerMask sectorDividerLayer;
    [SerializeField] private UnitController unit;
    [SerializeField] private AATAgentController agent;
    [SerializeField] private new Collider collider;

    private bool _traversing;
    private SectorDivider _currentSectorDivider;
    private Vector3 _agentDestinationRef;
    private Vector3 _fakeTraversePosition;
    private Vector3 _fakePositionStart;
    private float _targetDistanceSqr;

    public void AttemptTraverse()
    {
        if (_traversing) return;
        if (Physics.Raycast(transform.position, agent.DesiredDestination - transform.position, out var hit, sectorDividerLayer))
        {
            if (hit.collider.TryGetComponent<SectorDivider>(out var sectorDivider))
            {
                _agentDestinationRef = agent.DesiredDestination;
                _currentSectorDivider = sectorDivider;
                float angle = Vector3.Angle(_agentDestinationRef - transform.position, sectorDivider.transform.right);
                if (angle > 90) angle = 180 - angle;
                float dividerXDistance = sectorDivider.transform.localScale.x;
                float target = Mathf.Abs(dividerXDistance / Mathf.Cos(angle));
                _targetDistanceSqr = target * target;
                Traverse();
            }
        }
    }

    private void Traverse()
    {
        _traversing = true;
        AI.Deactivate();
        agent.DisableAgent(this);
        _fakeTraversePosition = transform.position;
        _fakePositionStart = _fakeTraversePosition;
        collider.enabled = false;
        InputManager.OnUpdate += MoveTraversable;
        transform.LookAt(_agentDestinationRef);
    }

    protected virtual void EndTraverse()
    {
        InputManager.OnUpdate -= MoveTraversable;
        unit.SetSector(unit.SectorController == _currentSectorDivider.Sectors[0] ? _currentSectorDivider.Sectors[1] : _currentSectorDivider.Sectors[0]);
        AI.Activate();
        agent.EnableAgent(this);
        transform.position = _fakeTraversePosition;
        collider.enabled = true;
        _traversing = false;
    }

    protected virtual void MoveTraversable()
    {
        _fakeTraversePosition += (_agentDestinationRef - _fakeTraversePosition).normalized * unitStatsModifier.CurrentUnitStatsData[EUnitFloatStats.TraverseSpeed] * Time.deltaTime;
        MoveRealPosition();
        if ((_fakeTraversePosition - _fakePositionStart).sqrMagnitude >= _targetDistanceSqr)
        {
            EndTraverse();
        }
    }

    protected virtual void MoveRealPosition()
    {
        transform.position = _fakeTraversePosition;
    }
}
