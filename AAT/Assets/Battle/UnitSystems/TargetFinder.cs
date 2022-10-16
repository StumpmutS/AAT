using System;
using Fusion;
using Utility.Scripts;

public class TargetFinder
{
    private TeamController _team;
    private float _range;
    private bool _lockTarget;

    public TargetFinder(TeamController team, float range, bool lockTarget)
    {
        _team = team;
        _range = range;
        _lockTarget = lockTarget;
    }
    
    private Hitbox _target;
    public StumpTarget Target => TargetHelper.ConvertToStumpTarget(_target);

    public void Tick()
    {
        var targetLayer = TeamManager.Instance.GetEnemyLayer(_team.GetTeamNumber());
        CollisionDetector.CheckRadius(_team.Runner, _team.Object.InputAuthority, _team.transform.position, _range, targetLayer, out _target, _lockTarget ? _target : null);
    }
}