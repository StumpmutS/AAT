using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Units/Unit Visual Components/Unit Animation Component")]
public class UnitAbilityAnimationComponent : UnitVisualComponent
{
    [SerializeField] private string abilityName;
    
    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        var animController = unit.GetComponent<UnitAnimationController>();
        animController.SetAbilityBool(abilityName);
    }
}