using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Data/Damage Component")]
public class ProjectileDamageComponentData : ProjectileComponentData
{
    public override void ActivateComponent(ProjectileController from, GameObject hit, float damage)
    {
        var health = hit.GetComponent<IHealth>();
        health?.ModifyHealth(-Mathf.Abs(damage));
    }
}
