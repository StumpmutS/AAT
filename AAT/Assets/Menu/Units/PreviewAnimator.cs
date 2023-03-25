using UnityEngine;

[RequireComponent(typeof(UnitAnimationController))]
public class PreviewAnimator : MonoBehaviour
{
    private UnitAnimationController _animation;

    private void Awake()
    {
        _animation = GetComponent<UnitAnimationController>();
    }

    private void Start()
    {
        _animation.SetMovement(0);
    }
}
