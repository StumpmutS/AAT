using System;
using Fusion;
using UnityEngine;
using Utility.Scripts;

public class TargetFinder : SimulationBehaviour
{
    private TeamController _team;
    
    private Hitbox _sightTarget;
    public Hitbox SightTarget => _sightTarget;
    private Hitbox _attackTarget;
    public Hitbox AttackTarget => _attackTarget;
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
        CollisionDetector.CheckRadius(Runner, Object.InputAuthority, transform.position, _innerRange, targetLayer, out _attackTarget, _attackTarget);
        CollisionDetector.CheckRadius(Runner, Object.InputAuthority, transform.position, _outerRange, targetLayer, out _sightTarget);
    }
}
