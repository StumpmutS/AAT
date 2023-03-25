using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Group))]
public class GroupAnimationController : StumpAnimationController
{
    private Group _group;
    private HashSet<StumpAnimationController> _animationControllers = new();

    private void Awake()
    {
        _group = GetComponent<Group>();
        _group.OnMembersChanged += ReCacheMemberAnimationControllers;
    }

    private void Start()
    {
        ReCacheMemberAnimationControllers();
    }

    private void ReCacheMemberAnimationControllers()
    {
        _animationControllers.Clear();

        foreach (var member in _group.GroupMembers)
        {
            if (member.TryGetComponent<StumpAnimationController>(out var animationController))
            {
                _animationControllers.Add(animationController);
            }
        }
    }

    public override void SetAnimationState(int value)
    {
        foreach (var animationController in _animationControllers)
        {
            animationController.SetAnimationState(value);
        }
    }

    public override void SetMovement(float value)
    {
        foreach (var animationController in _animationControllers)
        {
            animationController.SetMovement(value);
        }
    }

    public override void SetChase(bool value)
    {
        foreach (var animationController in _animationControllers)
        {
            animationController.SetChase(value);
        }
    }

    public override void SetAttack(bool value)
    {
        foreach (var animationController in _animationControllers)
        {
            animationController.SetAttack(value);
        }
    }

    public override void SetCrit(bool value)
    {
        foreach (var animationController in _animationControllers)
        {
            animationController.SetCrit(value);
        }
    }

    public override void SetAbilityBool(string abilityName, float time)
    {
        foreach (var animationController in _animationControllers)
        {
            animationController.SetAbilityBool(abilityName, time);
        }
    }
}