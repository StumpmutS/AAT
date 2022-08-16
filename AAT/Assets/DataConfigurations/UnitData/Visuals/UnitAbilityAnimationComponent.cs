using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Visual Components/Animation Component")]
public class UnitAbilityAnimationComponent : VisualComponent
{
    [SerializeField] private string abilityName;
    
    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        var animController = unit.GetComponent<UnitAnimationController>();
        animController.SetAbilityBool(abilityName);
    }
}
