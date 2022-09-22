using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHelper
{
    public static bool TargetRelation(TeamController from, TeamController other, ETargetRelation relation)
    {
        if (from == null || other == null) return relation == ETargetRelation.None;
        
        return relation switch
        {
            ETargetRelation.Owned => CheckOwned(from, other),
            ETargetRelation.Ally => CheckAlly(from, other),
            ETargetRelation.Enemy => CheckEnemy(from, other),
            _ => CheckNone(from, other)
        };
    }

    private static bool CheckEnemy(TeamController from, TeamController other)
    {
        return (from.GetTeamNumber() != other.GetTeamNumber()); //todo: allies
    }

    private static bool CheckAlly(TeamController from, TeamController other)
    {
        return false; //todo: allies
    }

    private static bool CheckOwned(TeamController from, TeamController other)
    {
        return from.GetTeamNumber() == other.GetTeamNumber();
    }

    private static bool CheckNone(TeamController from, TeamController other)
    {
        return false;
    }
}
