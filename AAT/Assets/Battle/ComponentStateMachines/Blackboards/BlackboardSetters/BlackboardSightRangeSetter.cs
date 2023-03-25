using System;
using UnityEngine;

public class BlackboardSightRangeSetter : BlackboardSetter<AiTransitionBlackboard>
{
    [SerializeField] private NetworkComponentStateContainer<AiTransitionBlackboard> container;

    private ChaseComponentState _chaseState;

    public override void Spawned()
    {
        if (container.TryGetComponentState(typeof(ChaseComponentState), out var state))
        {
            _chaseState = (ChaseComponentState) state;
        }
    }

    public override void FixedUpdateNetwork()
    {
        _blackboard.InSightRange = _chaseState.TargetFinder.Target.Hit != null;
    }
}