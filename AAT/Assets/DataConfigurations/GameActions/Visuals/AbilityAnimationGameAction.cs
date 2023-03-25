using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Actions/Visuals/Unit Animation")]
public class AbilityAnimationGameAction : AbilityGameAction
{
    [SerializeField] private string abilityName;
    
    public override void PerformAction(GameActionInfo info)
    {
        var animController = info.MainCaller.GetComponent<StumpAnimationController>();
        animController.SetAbilityBool(abilityName, Duration);
    }
}