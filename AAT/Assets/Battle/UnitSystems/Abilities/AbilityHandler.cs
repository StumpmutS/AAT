using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Utility.Scripts;

[RequireComponent(typeof(UnitController), typeof(UnitAnimationController))]
public class AbilityHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnAbilityIndexChanged))]
    private int AbilityIndex { get; set; } = -1;
    public static void OnAbilityIndexChanged(Changed<AbilityHandler> changed)
    {
        var abilityHandler = changed.Behaviour;
        if (abilityHandler.AbilityIndex < 0) return;
        var ability = abilityHandler._unitAbilityDataInfo[abilityHandler.AbilityIndex];

        abilityHandler._visualComponentHandler.ActivateVisuals(ability.UnitVisualComponents);
    }
    
    [SerializeField] private List<UnitAbilityData> unitAbilityData;

    private UnitController _unit;
    private VisualComponentHandler _visualComponentHandler;
    
    private List<UnitAbilityDataInfo> _unitAbilityDataInfo = new();
    private HashSet<UnitAbilityDataInfo> _abilitiesOnCooldown = new();
    public HashSet<UnitAbilityDataInfo> ActiveAbilities { get; private set; } = new();
    private HashSet<UnitAbilityDataInfo> _abilitiesCanBeCastOver = new();
    private HashSet<int> _abilityIndexesAwaitingInput = new();
    private HashSet<IntervalEventTimer<Tuple<AbilityComponent, Vector3>>> _repeatingTimers = new();
    private bool _checkInput;

    public event Action<bool> OnAbilityUsed = delegate { };

    private void Awake()
    {
        _unit = GetComponent<UnitController>();
        _visualComponentHandler = GetComponent<VisualComponentHandler>();
        foreach (var abilityData in unitAbilityData)
        {
            _unitAbilityDataInfo.Add(abilityData.UnitAbilityDataInfo);
        }
    }

    public override void FixedUpdateNetwork()
    {
        foreach (var timer in _repeatingTimers)
        {
            timer.Tick(Runner.DeltaTime);
        }
        
        if (!_checkInput || !Runner.IsServer) return;
        if (!GetInput<NetworkedInputData>(out var input)) return;
        if (input.LeftClickPosition == default) return;
        
        foreach(var index in _abilityIndexesAwaitingInput)
        {
            ActivateAbility(index, input.LeftClickPosition);
            _checkInput = false;
        }
        _abilityIndexesAwaitingInput.Clear();
    }

    private void TryAwaitAbilityInput(int abilityIndex)
    {
        if (Object.HasInputAuthority) RpcAwaitAbilityInput(abilityIndex);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcAwaitAbilityInput(int abilityIndex)
    {
        if (!Runner.IsServer) return;
        
        var info = _unitAbilityDataInfo[abilityIndex];

        if (info.TargetTimeOutTime > 0.1)
        {
            _abilityIndexesAwaitingInput.Add(abilityIndex);
            StartCoroutine(TargetedTimeOutTimer(info.TargetTimeOutTime, abilityIndex));
        }
        else
        {
            ActivateAbility(abilityIndex);
        }
    }

    private IEnumerator TargetedTimeOutTimer(float waitTime, int index)
    {
        _checkInput = true;
        yield return new WaitForSeconds(waitTime);
        if (!_abilityIndexesAwaitingInput.Contains(index)) yield break;
        
        _abilityIndexesAwaitingInput.Remove(index);
        if (_abilityIndexesAwaitingInput.Count < 1) _checkInput = false;
    }

    private void ActivateAbility(int abilityIndex, Vector3 point = default)
    {
        if (!RestrictionHelper.CheckRestrictions(_unitAbilityDataInfo[abilityIndex].Restrictions, _unit)) return;
        
        var info = _unitAbilityDataInfo[abilityIndex];
        if (_abilitiesOnCooldown.Contains(info) || CastingUninterruptable()) return;
        
        AbilityIndex = abilityIndex;
        StartAbilityTimers(info);
        foreach (var abilityComponent in info.AbilityComponents)
        {
            StartCoroutine(DelayComponentCoroutine(abilityComponent, point));
        }
    }

    private bool CastingUninterruptable() => _abilitiesCanBeCastOver.Count < ActiveAbilities.Count;

    private IEnumerator DelayComponentCoroutine(AbilityComponent abilityComponent, Vector3 point = default)
    {
        yield return new WaitForSeconds(abilityComponent.ComponentDelay);
        if (abilityComponent.Repeat)
        {
            var timer = new IntervalEventTimer<Tuple<AbilityComponent, Vector3>>(abilityComponent.RepeatIntervals,
                new Tuple<AbilityComponent, Vector3>(abilityComponent, point), ActivateComponent); //can pool if needed but should be fine
            _repeatingTimers.Add(timer);
            yield return new WaitForSeconds(abilityComponent.ComponentDuration);
            _repeatingTimers.Remove(timer);
            abilityComponent.DeactivateComponent(_unit);
        } 
        else 
        {
            abilityComponent.ActivateComponent(_unit, point);
            yield return new WaitForSeconds(abilityComponent.ComponentDuration);
            abilityComponent.DeactivateComponent(_unit);
        }
    }

    private void ActivateComponent(Tuple<AbilityComponent, Vector3> tuple)
    {
        tuple.Item1.ActivateComponent(_unit, tuple.Item2);
    }

    private void StartAbilityTimers(UnitAbilityDataInfo ability)
    {
        StartCoroutine(CoStartCooldown(ability));
        StartCoroutine(CoStartActiveTime(ability));
        if (ability.CanBeCastOver) StartCoroutine(CoStartCastOverTime(ability));
    }

    private IEnumerator CoStartCooldown(UnitAbilityDataInfo ability)
    {
        _abilitiesOnCooldown.Add(ability);
        yield return new WaitForSeconds(ability.CooldownTime);
        _abilitiesOnCooldown.Remove(ability);
    }

    private IEnumerator CoStartActiveTime(UnitAbilityDataInfo ability)
    {
        ActiveAbilities.Add(ability);
        OnAbilityUsed.Invoke(true);
        yield return new WaitForSeconds(ability.ActiveTime);
        ActiveAbilities.Remove(ability);
        _abilitiesCanBeCastOver.Remove(ability);
        OnAbilityUsed.Invoke(false);
        if (ActiveAbilities.Count < 1) AbilityIndex = -1;
    }

    private IEnumerator CoStartCastOverTime(UnitAbilityDataInfo ability)
    {
        yield return new WaitForSeconds(ability.AllowCastOverTimer);
        _abilitiesCanBeCastOver.Add(ability);
    }
}