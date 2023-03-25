using System.Linq;
using UnityEngine;

public class AnimatorAbility : MonoBehaviour
{
    [SerializeField] private AnimationEventHandler animationEventHandler;

    private void Ability(string abilityName)
    {
        if (!int.TryParse(abilityName.Last().ToString(), out var part))
        {
            Debug.LogError("Animator event for abilities must be in the form of \"{abilityName}{part}\". If there is only one part part still must be 0");
        }

        var actualName = abilityName.Remove(abilityName.Length - 1, 1);
        
        animationEventHandler.CallAnimationEvent(part, actualName);
    }
}