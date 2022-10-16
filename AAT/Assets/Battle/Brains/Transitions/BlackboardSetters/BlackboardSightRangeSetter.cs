using UnityEngine;

public class BlackboardSightRangeSetter : BlackboardSetter<AiTransitionBlackboard>
{
    [SerializeField] private NetworkComponentStateContainer<AiTransitionBlackboard> container;

    private ChaseComponentState _chaseState;

    private void Awake()
    {
        if (container.TryGetComponentState(typeof(AttackComponentState), out var state))
        {
            _chaseState = (ChaseComponentState) state;
        }
    }

    public override void FixedUpdateNetwork()
    { 
        _blackboard.InSightRange = _chaseState.TargetFinder.Target.Hit != null;
    }
}