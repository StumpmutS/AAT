using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Restrictions/Targeting Restriction")]
public class TargetingRestriction : Restriction
{
    [SerializeField] private ETeamRelation desiredRelation;
    
    public override bool CheckRestriction(IEnumerable<GameActionInfo> info)
    {
        return info.All(i => TeamRelations.TeamRelation(i.MainCaller.GetComponent<TeamController>(), i.Target.Hit.GetComponent<TeamController>(), desiredRelation));
    }
}