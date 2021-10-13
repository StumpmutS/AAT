using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private RectTransform abilityButtonsContainer;
    [SerializeField] private AbilityButtonController abilityButtonPrefab;

    public static AbilityManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static void DisplayAbilityButtons(List<UnitAbilityDataInfo> unitAbilityDataInfo, Action<int> abilityCallback)
    {
        for (int i = 0; i < instance.abilityButtonsContainer.childCount; i++)
        {
            Destroy(instance.abilityButtonsContainer.GetChild(i).gameObject);
        }

        for (int i = 0; i < unitAbilityDataInfo.Count; i++)
        {
            var instantiatedButton = Instantiate(instance.abilityButtonPrefab, instance.abilityButtonsContainer);
            instantiatedButton.Setup(unitAbilityDataInfo[i], abilityCallback, i);
        }

        instance.abilityButtonsContainer.gameObject.SetActive(true);
        InputManager.OnLeftCLick += instance.RemoveAbilityButtonDisplay;
    }

    private void RemoveAbilityButtonDisplay()
    {
        instance.abilityButtonsContainer.gameObject.SetActive(false);
        InputManager.OnLeftCLick -= instance.RemoveAbilityButtonDisplay;
    }
}
