using System.Collections;
using Fusion;
using UnityEngine;

public class KnockbackManager : SimulationBehaviour
{
    public static KnockbackManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;   
    }

    public void AddKnockback(Transform transformToBeKnocked, Vector3 direction, float distance, float speed, float lerpEndPercent)
    {
        StartCoroutine(CoStartKnockbackCoroutine(transformToBeKnocked, direction, distance, speed, lerpEndPercent));
    }

    private IEnumerator CoStartKnockbackCoroutine(Transform transformToBeKnocked, Vector3 direction, float distance, float speed, float lerpEndPercent)
    {
        if (!Runner.IsServer) yield break;
        
        var targetDistance = direction.normalized * distance;
        Vector3 previousMoveAmount = Vector3.zero;
        Vector3 moveAmount = Vector3.zero;
        float endValue = distance * 99 / 100;

        while (moveAmount.magnitude < endValue)
        {
            previousMoveAmount = moveAmount;
            moveAmount = Vector3.Lerp(moveAmount, targetDistance, speed * Runner.DeltaTime);
            transformToBeKnocked.position += moveAmount - previousMoveAmount;
            yield return 0;
        }
    }
}
