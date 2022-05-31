using UnityEngine;

public class AcceleratingProjectileController : ProjectileController
{
    [SerializeField] private float acceleration;
    
    protected override void MoveProjectile()
    {
        _rigidBody.velocity += initialDirection * (acceleration * Time.deltaTime);
    }
}
