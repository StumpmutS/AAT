using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AbilityButtonController : MonoBehaviour
{
    [SerializeField] private TMP_Text abilityNameText;
    [SerializeField] private SpringController activationSpring;

    private Button button;

    private Action<int> _abilityActivationCallback;
    private int _abilityIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ActivateAbility);
    }

    public void Setup(UnitAbilityDataInfo info, Action<int> callback, int abilityIndex)
    {
        _abilityActivationCallback = callback;
        _abilityIndex = abilityIndex;
        abilityNameText.text = info.AbilityName;
    }

    private void ActivateAbility()
    {
        _abilityActivationCallback?.Invoke(_abilityIndex);
    }

    public void ActivateButton()
    {
        activationSpring.SetTarget(0);
    }

    public void DeactivateButton()
    {
        activationSpring.SetTarget(1);
    }
}
