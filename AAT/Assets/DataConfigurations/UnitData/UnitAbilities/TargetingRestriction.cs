using Fusion;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Unit Data/Restrictions/Targeting Restriction")]
public class TargetingRestriction : Restriction
{
    [SerializeField] private ETeamRelation desiredRelation;
    
    public override bool CheckRestriction(UnitController unit)
    {
        if (!StumpNetworkRunner.Instance.Runner.TryGetInputForPlayer(unit.Object.InputAuthority, out NetworkedInputData input)) return false;
        CollisionDetector.CheckRadius(unit.Runner, unit.Object.InputAuthority, input.LeftClickPosition, .1f, Physics.AllLayers, out var hit);
        return TeamRelations.TeamRelation(unit.Team, hit.gameObject.GetComponent<TeamController>(), desiredRelation);
    }
}