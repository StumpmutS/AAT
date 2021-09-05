using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Dash Ability Component")]
public class DashAbilityComponentData : AbilityComponent
{
    [SerializeField] private Vector3 DashDistance;
    [SerializeField] private float DashSpeed;

    public override void ActivateComponent(GameObject gameObject)
    {
        Dash(gameObject);
    }

    private void Dash(GameObject gameObject)
    {
        var targetPosition = gameObject.transform.position +
            (gameObject.transform.right * DashDistance.x) +
            (gameObject.transform.up * DashDistance.y) +
            (gameObject.transform.forward * DashDistance.z);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, DashSpeed * Time.deltaTime);
    }
}