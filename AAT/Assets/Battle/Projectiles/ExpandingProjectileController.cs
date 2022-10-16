using UnityEngine;

public class ExpandingProjectileController : ProjectileController
{
    [SerializeField] private SphereCollider targetCollider;
    [SerializeField] private float radius;
    
    protected override void MoveProjectile()
    {
        base.MoveProjectile();
        targetCollider.radius *= Time.deltaTime * radius;
    }
}
