using UnityEngine;
using Utility.Scripts;

public class TargetFinder : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;

    private Collider _target;
    public Collider Target => _target;
    private float _innerRange;
    private float _outerRange;
    
    public void Init(float innerRange, float outerRange)
    {
        _innerRange = innerRange;
        _outerRange = outerRange;
    }

    private void Update()
    {
        if (!ColliderDetector.CheckRadius(transform.position, _innerRange, targetLayer, out _target, returnIfPresent: _target))
        {
            ColliderDetector.CheckRadius(transform.position, _outerRange, targetLayer, out _target);
        }
    }
}
