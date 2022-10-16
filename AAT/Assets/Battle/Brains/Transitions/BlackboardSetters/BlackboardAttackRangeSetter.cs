using UnityEngine;

public class BlackboardAttackRangeSetter : BlackboardSetter<AiTransitionBlackboard>
{
    [SerializeField] private NetworkComponentStateContainer<AiTransitionBlackboard> container;

    private AttackComponentState _attackState;

    private void Awake()
    {
        if (container.TryGetComponentState(typeof(AttackComponentState), out var state))
        {
            _attackState = (AttackComponentState) state;
        }
    }

    public override void FixedUpdateNetwork()
    { 
        _blackboard.InAttackRange = _attackState.TargetFinder.Target.Hit != null;
    }
}