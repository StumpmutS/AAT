using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility.Scripts;

[RequireComponent(typeof(NavMeshAgent))]
public class AATAgentController : MonoBehaviour
{
    [SerializeField] private UnitController unit;
    [SerializeField] private LayerMask ground;
    private NavMeshAgent _agent;
    private HashSet<MonoBehaviour> _disablers = new HashSet<MonoBehaviour>();

    public Vector3 DesiredDestination { get; private set; }
    public bool AgentEnabled => _agent.enabled;
    public float RemainingDistance => _agent.remainingDistance;
    public float Speed => _agent.speed;
    
    private Queue<Vector3> _queuedPoints = new Queue<Vector3>();
    private Queue<InteractableController> _queuedInteractables = new Queue<InteractableController>();
    private bool _pathSet;
    
    public event Action OnPathFinished = delegate { };
    public event Action OnPathSet = delegate { };

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        unit.OnInteractionFinished += CheckQueue;
    }

    public void SetDestination(Vector3 destination, bool dequeue = true)
    {
        DesiredDestination = destination;
        var fromSector = unit.SectorController;
        var targetSector = SectorFinder.FindSector(destination, 3, ground);
        if (fromSector == targetSector)
        {
            _agent.SetDestination(destination);
            OnPathSet.Invoke();
            _pathSet = false;
            if (dequeue) ClearQueue();
            return;
        }
        
        if (!SectorManager.Instance.PathBetween(fromSector, targetSector, out var points)) return;
        if (dequeue) ClearQueue();
        points[0].SetupInteractions(new List<UnitController>(){unit});
        for (var i = 1; i < points.Count; i++)
        {
            QueuePoint(Vector3.zero, points[i]);
        }
        QueuePoint(destination);
    }

    private void Update()
    {
        if (_agent.pathPending) return;
        if (_agent.hasPath) _pathSet = true;
        if (!_pathSet) return;
        if (_agent.remainingDistance >= .1f) return;
        _agent.ResetPath();
        _pathSet = false;
        OnPathFinished.Invoke();
        CheckQueue();
    }

    private void QueuePoint(Vector3 point, TeleportPoint teleportPoint = null)
    {
        _queuedPoints.Enqueue(point);
        _queuedInteractables.Enqueue(teleportPoint);
    }

    private void CheckQueue()
    { 
        if (_queuedInteractables.Count > 0 && _queuedInteractables.Peek() != null)
        {
            _queuedInteractables.Dequeue().SetupInteractions(new List<UnitController>() {unit});
            _queuedPoints.Dequeue();
            return;
        }

        if (_queuedPoints.Count > 0)
        {
            SetDestination(_queuedPoints.Dequeue(), false);
            _pathSet = false;
            _queuedInteractables.Dequeue();
        }
    }

    public void ClearQueue()
    {
        _queuedPoints.Clear();
        _queuedInteractables.Clear();
    }

    public void SetSpeed(float speed)
    {
        _agent.speed = speed;
    }

    public void EnableAgent(MonoBehaviour caller)
    {
        _disablers.Remove(caller);
        if (_disablers.Count <= 0) _agent.enabled = true;
    }

    public void DisableAgent(MonoBehaviour caller)
    {
        _disablers.Add(caller);
        _agent.enabled = false;
    }

    public void Warp(Vector3 desiredPos)
    {
        _agent.Warp(desiredPos);
    }
}
