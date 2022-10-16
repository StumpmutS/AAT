using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string movementFloatName, chaseBoolName, attackBoolName, critBoolName, idleRandomIntName, idleSuperRandomIntName, abilityIntName;
    [SerializeField] private float minIdleRandomTime, maxIdleRandomTime, idleSuperRandomChancePercent;
    [SerializeField] private int idleRandomNumber, idleSuperRandomNumber;

    private IEnumerator _randomIdleCoroutine;
    private bool _randomIdleCoroutineRunning;

    #region Setters
    public void SetMovement(float value)
    {
        if (value < .1f)
        {
            StartRandomIdle();
        } 
        else
        {
            StopRandomIdle();
        }
        animator.SetFloat(movementFloatName, value);
    }

    public void SetChase(bool value)
    {
        animator.SetBool(chaseBoolName, value);
    }

    public void SetAttack(bool value)
    {
        StopRandomIdle();
        animator.SetBool(attackBoolName, value);
        ResetBool(attackBoolName);
    }

    public void SetCrit(bool value)
    {
        StopRandomIdle();
        animator.SetBool(critBoolName, value);
        ResetBool(critBoolName);
    }

    public void SetAbilityBool(string abilityName)
    {
        StopRandomIdle();
        animator.SetBool(abilityName, true);
        ResetBool(abilityName);
    }
    #endregion

    #region RandomIdleCoroutine
    private void StartRandomIdle()
    {
        if (_randomIdleCoroutineRunning) return;
        _randomIdleCoroutine = StartRandomIdleCoroutine();
        StartCoroutine(_randomIdleCoroutine);
    }

    private void StopRandomIdle()
    {
        if (_randomIdleCoroutineRunning)
        {
            StopCoroutine(_randomIdleCoroutine);
            _randomIdleCoroutineRunning = false;
        }
    }

    private IEnumerator StartRandomIdleCoroutine()
    {
        _randomIdleCoroutineRunning = true;
        float secondsToWait = Random.Range(minIdleRandomTime, maxIdleRandomTime);
        yield return new WaitForSeconds(secondsToWait);
        if (Random.Range(0, 100) < idleSuperRandomChancePercent)
        {
            animator.SetInteger(idleSuperRandomIntName, Random.Range(0, idleSuperRandomNumber - 1));
            yield return StartCoroutine(CoResetInt(idleSuperRandomIntName));
        } 
        else
        {
            animator.SetInteger(idleRandomIntName, Random.Range(0, idleRandomNumber - 1));
            yield return StartCoroutine(CoResetInt(idleRandomIntName));
        }
        _randomIdleCoroutineRunning = false;
        StartRandomIdle();
    }
    #endregion

    #region Resets
    private void ResetBool(string boolName)
    {
        StartCoroutine(ResetBoolCoroutine(boolName));
    }

    private IEnumerator ResetBoolCoroutine(string boolName)
    {
        yield return 0;
        animator.SetBool(boolName, false);
    }

    private void ResetInt(string intName)
    {
        StartCoroutine(CoResetInt(intName));
    }

    private IEnumerator CoResetInt(string intName)
    {
        yield return 0;
        animator.SetInteger(intName, -1);
    }
    #endregion
}
