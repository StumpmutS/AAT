using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController), typeof(UnitAnimationController))]
public class AbilityHandler : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private List<UnitAbilityData> unitAbilityData;
    
    private List<UnitAbilityDataInfo> _unitAbilityDataInfo = new List<UnitAbilityDataInfo>();
    private Dictionary<UnitAbilityDataInfo, bool> _abilitiesByActiveState = new Dictionary<UnitAbilityDataInfo, bool>();
    private HashSet<int> _abilityIndexesAwaitingInput = new HashSet<int>();
    private bool _checkGroundSubscribed;
    private UnitController _unitController;
    private UnitAnimationController _unitAnimationController;

    public event Action<bool> OnAbilityUsed = delegate { };

    private void Awake()
    {
        _unitController = GetComponent<UnitController>();
        _unitAnimationController = GetComponent<UnitAnimationController>();
        _unitController.OnSelect += SendDisplayData;
        foreach (var abilityData in unitAbilityData)
        {
            _unitAbilityDataInfo.Add(abilityData.UnitAbilityDataInfo);
        }
    }

    public void AddAbility(UnitAbilityData unitAbility)
    {
        unitAbilityData.Add(unitAbility);
    }

    private void AwaitAbilityInput(int abilityIndex)
    {
        var info = _unitAbilityDataInfo[abilityIndex];

        if (info.TargetTimeOutTime > 0)
        {
            SubscribeCheckGround();
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
        yield return new WaitForSeconds(waitTime);
        if (_abilityIndexesAwaitingInput.Contains(index))
        {
            UnsubscribeCheckGround();
            _abilityIndexesAwaitingInput.Remove(index);
        }
    }

    private void SubscribeCheckGround()
    {
        if (_checkGroundSubscribed) return;
        InputManager.OnLeftCLick += CheckGround;
        _checkGroundSubscribed = true;
    }

    private void UnsubscribeCheckGround()
    {
        if (!_checkGroundSubscribed) return;
        InputManager.OnLeftCLick += CheckGround;
        _checkGroundSubscribed = false;
    }

    private void CheckGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, groundLayer))
        {
            foreach(var index in _abilityIndexesAwaitingInput)
            {
                ActivateAbility(index, hit.point);
                UnsubscribeCheckGround();
            }
            _abilityIndexesAwaitingInput.Clear();
        }
    }

    private void ActivateAbility(int abilityIndex, Vector3 point = default)
    {
        var info = _unitAbilityDataInfo[abilityIndex];
        if (_abilitiesByActiveState.ContainsKey(info))
            if (_abilitiesByActiveState[info]) return;
        StartCooldown(info);
        foreach (var abilityComponent in info.AbilityComponents)
        {
            StartCoroutine(DelayComponentCoroutine(abilityComponent, point));
        }
        _unitAnimationController.SetAbility(abilityIndex);
    }

    private IEnumerator DelayComponentCoroutine(AbilityComponent abilityComponent, Vector3 point = default)
    {
        yield return new WaitForSeconds(abilityComponent.ComponentDelay);
        if (abilityComponent.Repeat)
        {
            var repeatCoroutine = RepeatComponentActivationCoroutine(abilityComponent, point);
            StartCoroutine(repeatCoroutine);
            yield return new WaitForSeconds(abilityComponent.ComponentDuration);
            StopCoroutine(repeatCoroutine);
            abilityComponent.DeactivateComponent(_unitController);
        } 
        else 
        {
            abilityComponent.ActivateComponent(_unitController, point);
            abilityComponent.DeactivateComponent(_unitController);
        }
    }

    private IEnumerator RepeatComponentActivationCoroutine(AbilityComponent abilityComponent, Vector3 point = default)
    {
        while (true)
        {
            abilityComponent.ActivateComponent(_unitController, point);
            yield return new WaitForSeconds(abilityComponent.RepeatIntervals);
        }
    }

    private void StartCooldown(UnitAbilityDataInfo ability)
    {
        StartCoroutine(StartCooldownCoroutine(ability));
        StartCoroutine(StartActiveTimeCoroutine(ability));
    }

    private IEnumerator StartCooldownCoroutine(UnitAbilityDataInfo ability)
    {
        _abilitiesByActiveState[ability] = true;
        yield return new WaitForSeconds(ability.CooldownTime);
        _abilitiesByActiveState[ability] = false;
    }

    private IEnumerator StartActiveTimeCoroutine(UnitAbilityDataInfo ability)
    {
        OnAbilityUsed.Invoke(true);
        yield return new WaitForSeconds(ability.ActiveTime);
        OnAbilityUsed.Invoke(false);
    }

    private void SendDisplayData()
    {
        AbilityManager.DisplayAbilityButtons(_unitAbilityDataInfo, AwaitAbilityInput);
    }
}
