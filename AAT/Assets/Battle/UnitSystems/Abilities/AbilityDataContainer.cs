using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IAbilitySystem))]
public class AbilityDataContainer : MonoBehaviour, IActionCreator
{
    [SerializeField] private string category;
    [SerializeField] private List<UnitAbilityData> unitAbilityData;
    public List<UnitAbilityData> UnitAbilityData => unitAbilityData;

    private IAbilitySystem _abilitySystem;
    private HashSet<UserAction> _activeActions;

    private void Awake()
    {
        _abilitySystem = GetComponent<IAbilitySystem>();
    }

    public List<UserAction> GetActions()
    {
        List<UserAction> actions = new();
        foreach (var ability in unitAbilityData)
        {
            actions.Add(CreateAction(ability.UnitAbilityDataInfo));
        }

        return actions;
    }

    private UserAction CreateAction(UnitAbilityDataInfo ability)
    {
        return new UserAction(category, ESubCategory.Ability, ability.AbilityName, ability.Icon, ActionCallBack, ability, ability.KeyCode);
    }

    private void ActionCallBack(object obj)
    {
        if (obj is not UnitAbilityDataInfo ability) return;

        _abilitySystem.PrepareAbility(ability); //TODO: cast ability
    }
}