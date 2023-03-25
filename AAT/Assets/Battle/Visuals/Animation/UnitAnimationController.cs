using System.Collections;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitAnimationController : StumpAnimationController
{
    [SerializeField] private NetworkMecanimAnimator networkAnimator;
    [SerializeField, Tooltip("Indexes should correspond to animator state indexes")] private string[] animatorStateNames;
    [SerializeField] private string animatorStateParameterName = "AnimatorState", movementFloatName, chaseBoolName, attackBoolName, critBoolName, idleRandomIntName, idleSuperRandomIntName;
    [SerializeField] private float minIdleRandomTime, maxIdleRandomTime, idleSuperRandomChancePercent;
    [SerializeField] private int idleRandomNumber, idleSuperRandomNumber;

    private IEnumerator _randomIdleCoroutine;
    private bool _randomIdleCoroutineRunning;

    #region Setters
    public override void SetAnimationState(int value)
    {
        networkAnimator.Animator.SetInteger(animatorStateParameterName, value);
    }
    
    public override void SetMovement(float value)
    {
        if (value < .1f)
        {
            StartRandomIdle();
        } 
        else
        {
            StopRandomIdle();
        }
        networkAnimator.Animator.SetFloat(movementFloatName, value);
    }

    public override void SetChase(bool value)
    {
        networkAnimator.Animator.SetBool(chaseBoolName, value);
    }

    public override void SetAttack(bool value)
    {
        StopRandomIdle();
        networkAnimator.Animator.SetBool(attackBoolName, value);
        ResetBool(attackBoolName);
    }

    public override void SetCrit(bool value)
    {
        StopRandomIdle();
        networkAnimator.Animator.SetBool(critBoolName, value);
        ResetBool(critBoolName);
    }

    public override void SetAbilityBool(string abilityName, float time)
    {
        StopRandomIdle();
        int animatorStateIndex = networkAnimator.Animator.GetInteger(animatorStateParameterName);
        networkAnimator.Animator.Play(animatorStateNames[animatorStateIndex] + abilityName);
        
        networkAnimator.Animator.SetBool(abilityName, true);
        ResetBool(abilityName, time);
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
            networkAnimator.Animator.SetInteger(idleSuperRandomIntName, Random.Range(0, idleSuperRandomNumber - 1));
            yield return StartCoroutine(CoResetInt(idleSuperRandomIntName));
        } 
        else
        {
            networkAnimator.Animator.SetInteger(idleRandomIntName, Random.Range(0, idleRandomNumber - 1));
            yield return StartCoroutine(CoResetInt(idleRandomIntName));
        }
        _randomIdleCoroutineRunning = false;
        StartRandomIdle();
    }
    #endregion

    #region Resets
    private void ResetBool(string boolName, float time = 0)
    {
        StartCoroutine(ResetBoolCoroutine(boolName, time));
    }

    private IEnumerator ResetBoolCoroutine(string boolName, float time)
    {
        yield return new WaitForSeconds(time);
        networkAnimator.Animator.SetBool(boolName, false);
    }

    private void ResetInt(string intName)
    {
        StartCoroutine(CoResetInt(intName));
    }

    private IEnumerator CoResetInt(string intName)
    {
        yield return 0;
        networkAnimator.Animator.SetInteger(intName, -1);
    }
    #endregion
}