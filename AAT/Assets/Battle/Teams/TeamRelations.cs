using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamRelations
{
    public static bool TeamRelation(TeamController from, TeamController other, ETeamRelation relation)
    {
        if (from == null || other == null) return relation == ETeamRelation.None;
        
        return relation switch
        {
            ETeamRelation.Owned => CheckOwned(from, other),
            ETeamRelation.Ally => CheckAlly(from, other),
            ETeamRelation.Enemy => CheckEnemy(from, other),
            _ => CheckNone(from, other)
        };
    }

    public static ETeamRelation GetTeamRelation(TeamController from, TeamController other)
    {
        if (CheckOwned(from, other)) return ETeamRelation.Owned;
        if (CheckAlly(from, other)) return ETeamRelation.Ally;
        if (CheckEnemy(from, other)) return ETeamRelation.Enemy;
        return ETeamRelation.None;
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
