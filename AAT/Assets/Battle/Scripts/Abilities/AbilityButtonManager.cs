using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityButtonManager : MonoBehaviour
{
    [SerializeField] private List<AbilityButtonController> abilityButtons;

    public static AbilityButtonManager Instance { get; private set; }

    private AbilityHandler _currentHandler;
    private HashSet<AbilityHandler> _handlersWaiting = new();

    private void Awake() => Instance = this;

    private void Start()
    {
        DeactivateButtons();
    }

    public void DisplayAbilityButtons(AbilityHandler abilityHandler, List<UnitAbilityDataInfo> unitAbilityDataInfo, Action<int> abilityCallback)
    {
        if (_currentHandler != null && abilityHandler != _currentHandler)
        {
            _handlersWaiting.Add(abilityHandler);
            return;
        }
        
        if (_currentHandler != null) _handlersWaiting.Add(_currentHandler);
        _currentHandler = abilityHandler;
        
        DeactivateButtons();
        
        for (int i = 0; i < unitAbilityDataInfo.Count; i++)
        {
            abilityButtons[i].Setup(unitAbilityDataInfo[i], abilityCallback, i);
            abilityButtons[i].ActivateButton();
        }
    }

    public void RemoveDisplayData(AbilityHandler abilityHandler)
    {
        if (abilityHandler != _currentHandler)
        {
            _handlersWaiting.Remove(abilityHandler);
            return;
        }
        DeactivateButtons();
        _currentHandler = null;

        var nextHandler = _handlersWaiting.FirstOrDefault();
        if (nextHandler != null)
        {
            _currentHandler = nextHandler;
            nextHandler.RequestDisplay();
            _handlersWaiting.Remove(nextHandler);
        }
    }

    private void DeactivateButtons()
    {
        foreach (var button in abilityButtons)
        {
            button.DeactivateButton();
        }
    }
}
