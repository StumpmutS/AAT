using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingProjectileController : ProjectileController
{
    [SerializeField] private float acceleration;
    
    protected override void MoveProjectile()
    {
        base.MoveProjectile();
        _rigidBody.velocity += initialDirection * acceleration * Time.deltaTime;
    }
}
