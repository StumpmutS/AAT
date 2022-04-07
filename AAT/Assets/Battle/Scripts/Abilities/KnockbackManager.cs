using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackManager : MonoBehaviour
{
    public static KnockbackManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;   
    }

    public void AddKnockback(Transform transformToBeKnocked, Vector3 direction, float distance, float speed, float lerpEndPercent)
    {
        StartCoroutine(StartKnockbackCoroutine(transformToBeKnocked, direction, distance, speed, lerpEndPercent));
    }

    private IEnumerator StartKnockbackCoroutine(Transform transformToBeKnocked, Vector3 direction, float distance, float speed, float lerpEndPercent)
    {
        var targetDistance = direction.normalized * distance;
        Vector3 previousMoveAmount = Vector3.zero;
        Vector3 moveAmount = Vector3.zero;
        float endValue = distance * (lerpEndPercent / 100);

        while (moveAmount.magnitude < endValue)
        {
            previousMoveAmount = moveAmount;
            moveAmount = Vector3.Lerp(moveAmount, targetDistance, speed * Time.deltaTime);
            transformToBeKnocked.position += moveAmount - previousMoveAmount;
            yield return 0;
        }
    }
}
