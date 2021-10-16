using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AbilityButtonController : MonoBehaviour
{
    [SerializeField] private TMP_Text abilityNameText;

    private Button button;

    private Action<int> _abiilityActivationCallback;
    private int _abilityIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ActivateAbility);
    }

    public void Setup(UnitAbilityDataInfo info, Action<int> callback, int abilityIndex)
    {
        _abiilityActivationCallback = callback;
        _abilityIndex = abilityIndex;
        abilityNameText.text = info.AbilityName;
    }

    private void ActivateAbility()
    {
        _abiilityActivationCallback.Invoke(_abilityIndex);
    }
}
