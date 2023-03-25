using UnityEngine;

public abstract class StumpAnimationController : MonoBehaviour
{
    public abstract void SetAnimationState(int value);
    
    public abstract void SetMovement(float value);

    public abstract void SetChase(bool value);

    public abstract void SetAttack(bool value);

    public abstract void SetCrit(bool value);

    public abstract void SetAbilityBool(string abilityName, float time);
}