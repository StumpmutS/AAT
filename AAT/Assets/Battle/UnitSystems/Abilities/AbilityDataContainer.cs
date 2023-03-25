using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(IAbilitySystem))]
public class AbilityDataContainer : NetworkBehaviour, IActionCreator
{
    [SerializeField] private Selectable selectable;
    [SerializeField] private string category;
    [SerializeField] private List<UnitAbilityData> unitAbilityData;

    private IAbilitySystem _abilitySystem;
    private UnitAbilityData _selectedAbility;

    private void Awake()
    {
        _abilitySystem = GetComponent<IAbilitySystem>();
    }

    public List<UserAction> GetActions()
    {
        List<UserAction> actions = new();
        foreach (var ability in unitAbilityData)
        {
            actions.Add(CreateAction(ability));
        }

        return actions;
    }

    private UserAction CreateAction(UnitAbilityData ability)
    {
        return new UserAction(this, category, ESubCategory.Ability, ability.UnitAbilityDataInfo.UserActionInfo.Label,
            ability.UnitAbilityDataInfo.UserActionInfo.Icon, ActionSelectedCallback, ActionDeselectedCallback, ability,
            ability.UnitAbilityDataInfo.UserActionInfo.KeyCode);
    }

    private void ActionSelectedCallback(object obj)
    {
        if (obj is not UnitAbilityData ability) return;

        _selectedAbility = ability;
        selectable.InputAwaiter.StartAwaitInput(this);
        _abilitySystem.PrepareAbility(ability);
    }

    public override void FixedUpdateNetwork()
    {
        if (Player.LeftClickTarget == null || _selectedAbility == null) return;
        
        selectable.InputAwaiter.StopAwaitInput(this);
        _abilitySystem.CastAbility(_selectedAbility, Player.LeftClickTarget);
        _selectedAbility = null;
    }

    private void ActionDeselectedCallback(object obj)
    {
        selectable.InputAwaiter.StopAwaitInput(this);
        if (obj is not UnitAbilityData ability) return;
        
        _selectedAbility = null;
        _abilitySystem.UnPrepareAbility(ability);
    }
}
