using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private RectTransform abilityButtonsContainer;
    [SerializeField] private AbilityButtonController abilityButtonPrefab;

    public static AbilityManager Instance { get; private set; }

    private void Awake() => Instance = this;

    public void DisplayAbilityButtons(List<UnitAbilityDataInfo> unitAbilityDataInfo, Action<int> abilityCallback)
    {
        for (int i = 0; i < Instance.abilityButtonsContainer.childCount; i++)
        {
            Destroy(Instance.abilityButtonsContainer.GetChild(i).gameObject);
        }

        for (int i = 0; i < unitAbilityDataInfo.Count; i++)
        {
            var instantiatedButton = Instantiate(Instance.abilityButtonPrefab, Instance.abilityButtonsContainer);
            instantiatedButton.Setup(unitAbilityDataInfo[i], abilityCallback, i);
        }

        Instance.abilityButtonsContainer.gameObject.SetActive(true);
        InputManager.OnLeftCLickUp += Instance.RemoveAbilityButtonDisplay;
    }

    private void RemoveAbilityButtonDisplay()
    {
        if (StumpEventSystemManagerReference.Instance.OverUI()) return;
        abilityButtonsContainer.gameObject.SetActive(false);
        InputManager.OnLeftCLickUp -= RemoveAbilityButtonDisplay;
    }
}
