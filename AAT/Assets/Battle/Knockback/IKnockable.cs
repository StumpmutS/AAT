using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockable
{
    public void Knockback(Vector3 direction, float power);
}
