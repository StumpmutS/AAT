using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class AbilityHandler : MonoBehaviour
{
    [SerializeField] private List<UnitAbilityData> unitAbilityData;
    private List<UnitAbilityDataInfo> unitAbilityDataInfo = new List<UnitAbilityDataInfo>();
    private Dictionary<UnitAbilityDataInfo, bool> abilitiesByActiveState = new Dictionary<UnitAbilityDataInfo, bool>();

    public event Action<bool> OnAbilityUsed = delegate { };

    private UnitController unitController;

    private void Awake()
    {
        unitController = GetComponent<UnitController>();
        unitController.OnSelect += SendDisplayData;
        foreach (var abilityData in unitAbilityData)
        {
            unitAbilityDataInfo.Add(abilityData.unitAbilityDataInfo);
        }
    }

    public void AddAbility(UnitAbilityData unitAbility)
    {
        unitAbilityData.Add(unitAbility);
    }

    private void ActivateAbility(int abilityIndex)
    {
        var info = unitAbilityDataInfo[abilityIndex];
        if (abilitiesByActiveState.ContainsKey(info))
            if (abilitiesByActiveState[info] == true) return;
        StartCooldown(info);
        foreach (var abilityComponent in info.AbilityComponents)
        {
            StartCoroutine(DelayComponentCoroutine(abilityComponent));
        }
    }

    private IEnumerator DelayComponentCoroutine(AbilityComponent abilityComponent)
    {
        yield return new WaitForSeconds(abilityComponent.ComponentDelay);
        if (abilityComponent.Repeat)
        {
            var repeatCoroutine = RepeatComponentActivationCoroutine(abilityComponent);
            StartCoroutine(repeatCoroutine);
            yield return new WaitForSeconds(abilityComponent.ComponentDuration);
            StopCoroutine(repeatCoroutine);
            abilityComponent.DeactivateComponent(gameObject);
        } else 
        {
            abilityComponent.ActivateComponent(gameObject);
            abilityComponent.DeactivateComponent(gameObject);
        }
    }

    private IEnumerator RepeatComponentActivationCoroutine(AbilityComponent abilityComponent)
    {
        while (true)
        {
            abilityComponent.ActivateComponent(gameObject);
            yield return 0;
        }
    }

    private void StartCooldown(UnitAbilityDataInfo ability)
    {
        StartCoroutine(StartCooldownCoroutine(ability));
        StartCoroutine(StartActiveTimeCoroutine(ability));
    }

    private IEnumerator StartCooldownCoroutine(UnitAbilityDataInfo ability)
    {
        abilitiesByActiveState[ability] = true;
        yield return new WaitForSeconds(ability.CooldownTime);
        abilitiesByActiveState[ability] = false;
    }

    private IEnumerator StartActiveTimeCoroutine(UnitAbilityDataInfo ability)
    {
        OnAbilityUsed.Invoke(true);
        yield return new WaitForSeconds(ability.ActiveTime);
        OnAbilityUsed.Invoke(false);
    }

    private void SendDisplayData()
    {
        AbilityManager.DisplayAbilityButtons(unitAbilityDataInfo, ActivateAbility);
    }
}
