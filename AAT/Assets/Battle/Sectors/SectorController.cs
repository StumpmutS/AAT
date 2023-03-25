using System;
using System.Collections;
using System.Collections.Generic;
using Utility.Scripts;
using Fusion;
using UnityEngine;

public class SectorController : NetworkBehaviour
{
    [Networked(OnChanged = (nameof(OnCaptureChanged)))] 
    public NetworkDictionary<int, float> TeamCapturePercentages => default;
    public static void OnCaptureChanged(Changed<SectorController> changed)
    {
        var percentLeader = changed.Behaviour.TeamCapturePercentages.MaxKeyByValue();
        if (percentLeader == changed.Behaviour._percentLeaderTeam) return;

        changed.Behaviour._percentLeaderTeam = percentLeader;
        changed.Behaviour.OnPercentLeaderTeamChanged.Invoke(percentLeader);
    }
    [Networked] public NetworkDictionary<int, float> TeamPowers => default;

    [SerializeField] private float captureSpeed;
    public float CaptureSpeed => captureSpeed;
    
    private Dictionary<StatsManager, float> _storedStats = new();
    private int _percentLeaderTeam;

    private int _ownerTeam
    {
        get
        {
            return Runner.GetPlayerObject(Object.InputAuthority).GetComponent<TeamController>().GetTeamNumber();
        }
        set
        {
            Object.AssignInputAuthority(TeamManager.Instance.GetPlayerForTeam(value));
            OnCaptured.Invoke(value);
        }
    }
    
    public event Action<int> OnPercentLeaderTeamChanged = delegate { }; 
    public event Action<int> OnCaptured = delegate { };

    public void Init(int playerTeamNumber)
    {
        if (!Runner.IsServer) return;
        
        _ownerTeam = playerTeamNumber;
        TeamCapturePercentages.Set(playerTeamNumber, 100);
        TeamPowers.Set(playerTeamNumber, 0);
    }

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer || TeamPowers.Count < 1) return;
        
        var strongestTeamPresence = TeamPowers.MaxKeyByValue();
        bool capturable = true;
        
        foreach (var kvp in TeamCapturePercentages)
        {
            if (kvp.Key == strongestTeamPresence) continue;

            TeamCapturePercentages.Set(kvp.Key, Mathf.MoveTowards(kvp.Value, 0, captureSpeed * Runner.DeltaTime));
            if (TeamCapturePercentages.Get(kvp.Key) > 0) capturable = false;
        }

        if (!capturable) return;

        if (!TeamCapturePercentages.ContainsKey(strongestTeamPresence))
        {
            TeamCapturePercentages.Set(strongestTeamPresence, 0);
        }
        TeamCapturePercentages.Set(strongestTeamPresence, Mathf.MoveTowards(TeamCapturePercentages.Get(strongestTeamPresence), 100, captureSpeed * Runner.DeltaTime));
        if (TeamCapturePercentages.Get(strongestTeamPresence) < 100) return;

        _ownerTeam = strongestTeamPresence;
    }

    public void AddMember(Component component)
    {
        if (!Runner.IsServer) return;
        
        var memberStats = component.GetComponent<StatsManager>();
        if (_storedStats.ContainsKey(memberStats)) return;
        
        _storedStats[memberStats] = memberStats.CalculatePower();
        var teamNumber = component.GetComponent<TeamController>().GetTeamNumber();
        if (!TeamPowers.ContainsKey(teamNumber))
        {
            TeamPowers.Set(teamNumber, 0);
        }
        TeamPowers.Set(teamNumber, TeamPowers.Get(teamNumber) + _storedStats[memberStats]);
        memberStats.OnRefreshStats += HandleStatsChanged;
    }

    private void HandleStatsChanged(StatsManager statsManager)
    {
        var teamNumber = statsManager.GetComponent<TeamController>().GetTeamNumber();
        TeamPowers.Set(teamNumber, TeamPowers.Get(teamNumber) - _storedStats[statsManager]);
        _storedStats[statsManager] = statsManager.CalculatePower();
        TeamPowers.Set(teamNumber, TeamPowers.Get(teamNumber) + _storedStats[statsManager]);
    }

    public void RemoveMember(Component component)
    {
        if (!Runner.IsServer) return;
        
        var memberStats = component.GetComponent<StatsManager>();
        if (!_storedStats.ContainsKey(memberStats)) return;
        
        var teamNumber = component.GetComponent<TeamController>().GetTeamNumber();
        TeamPowers.Set(teamNumber, TeamPowers.Get(teamNumber) - _storedStats[memberStats]);
        memberStats.OnRefreshStats -= HandleStatsChanged;
        _storedStats.Remove(memberStats);
    }
}
