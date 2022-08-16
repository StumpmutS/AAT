using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHelper
{
    public static bool TargetRelation(TeamController from, TeamController other, ETargetRelation relation)
    {
        switch (relation)
        {
            default:
                return CheckNone(from, other);
            case ETargetRelation.Owned :
                return CheckOwned(from, other);
            case ETargetRelation.Ally :
                return CheckAlly(from, other);
            case ETargetRelation.Enemy :
                return CheckEnemy(from, other);
        }
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
