using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class SectorController : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnSectorCaptureChange))] 
    private PlayerRef Capturer { get; set; }
    [Networked(OnChanged = nameof(OnSectorCaptureChange))] 
    private int SectorCapturePercent { get; set; }
    public static void OnSectorCaptureChange(Changed<SectorController> changed)
    {
        changed.Behaviour.OnSectorCaptureChanged.Invoke(changed.Behaviour.Capturer, changed.Behaviour.SectorCapturePercent);
    }
    
    [Networked(OnChanged = nameof(OnSectorPowerChange))]
    public int SectorPower { get; private set; }
    public static void OnSectorPowerChange(Changed<SectorController> changed)
    {
        changed.Behaviour.OnSectorPowerChanged.Invoke(changed.Behaviour);
    }

    [SerializeField] private float captureSpeed;
    public float CaptureSpeed => captureSpeed;
    
    private Dictionary<int, List<UnitController>> _unitsByTeamNumber = new();
    private float _cachedExactCaptured;

    public event Action<PlayerRef, int> OnSectorCaptureChanged = delegate { };
    public event Action<SectorController> OnSectorPowerChanged = delegate { };
    
    public void Init(PlayerRef player)
    {
        if (!Runner.IsServer) return;   
        
        Object.AssignInputAuthority(player);
        SectorCapturePercent = 100;
        Capturer = Object.InputAuthority;
    }

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer || Capturer == default) return;

        if (Object.InputAuthority == default || (Object.InputAuthority == Capturer && SectorCapturePercent < 100))
        {
            _cachedExactCaptured += Runner.DeltaTime * captureSpeed;
            var floored = Mathf.FloorToInt(_cachedExactCaptured);
            if (floored > SectorCapturePercent) SectorCapturePercent = floored;
            
            if (SectorCapturePercent >= 100)
            {
                _cachedExactCaptured = 100;
                SectorCapturePercent = 100;
                Object.AssignInputAuthority(Capturer);
                SectorPower = 0;
                foreach (var unit in _unitsByTeamNumber.Values.ToList()[0])
                {
                    ModifySectorPower(unit.Stats.CalculatePower());
                }
            }
            return;
        }

        if (Object.InputAuthority == Capturer) return;
        
        _cachedExactCaptured -= Runner.DeltaTime * captureSpeed;
        var ceiled = Mathf.CeilToInt(_cachedExactCaptured);
        if (ceiled < SectorCapturePercent) SectorCapturePercent = ceiled;
        
        if (SectorCapturePercent <= 0)
        {
            _cachedExactCaptured = 0;
            SectorCapturePercent = 0;
            Object.AssignInputAuthority(default);
            SectorPower = 0;
        }
    }

    public void AddUnit(UnitController unit)
    {
        if (!Runner.IsServer) return;

        Debug.Log(unit.Team.GetTeamNumber());
        if (!_unitsByTeamNumber.ContainsKey(unit.Team.GetTeamNumber()))
        {
            _unitsByTeamNumber[unit.Team.GetTeamNumber()] = new List<UnitController>();
        }
        
        _unitsByTeamNumber[unit.Team.GetTeamNumber()].Add(unit);
        ModifySectorPower(unit.Stats.CalculatePower());
        CheckCapture();
    }

    public void RemoveUnit(UnitController unit)
    {
        if (!Runner.IsServer) return;

        _unitsByTeamNumber[unit.Team.GetTeamNumber()].Remove(unit);
        if (_unitsByTeamNumber[unit.Team.GetTeamNumber()].Count < 1) _unitsByTeamNumber.Remove(unit.Team.GetTeamNumber());
        ModifySectorPower(-unit.Stats.CalculatePower());
        CheckCapture();
    }

    private void CheckCapture()
    {
        if (_unitsByTeamNumber.Count == 1)
        {
            Capturer = TeamManager.Instance.GetPlayerForTeam(_unitsByTeamNumber.Keys.ToList()[0]);
        }
        else Capturer = default;
    }
    
    private void ModifySectorPower(int amount)
    {
        if (Object.InputAuthority == default) return;
        SectorPower += amount;
    }
}
