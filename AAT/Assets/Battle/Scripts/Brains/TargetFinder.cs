using System;
using UnityEngine;
using Utility.Scripts;

public class TargetFinder : MonoBehaviour
{
    private TeamController _team;
    
    private Collider _sightTarget;
    public Collider SightTarget => _sightTarget;
    private Collider _attackTarget;
    public Collider AttackTarget => _attackTarget;
    private float _innerRange;
    private float _outerRange;

    private void Awake()
    {
        _team = GetComponent<TeamController>();
    }

    public void Init(float innerRange, float outerRange)
    {
        _innerRange = innerRange;
        _outerRange = outerRange;
    }

    private void Update()
    {
        var targetLayer = TeamManager.Instance.GetEnemyLayer(_team.GetTeamNumber());
        CollisionDetector.CheckRadius(transform.position, _innerRange, targetLayer, out _attackTarget, returnIfPresent: _attackTarget);
        CollisionDetector.CheckRadius(transform.position, _outerRange, targetLayer, out _sightTarget);
    }
}
