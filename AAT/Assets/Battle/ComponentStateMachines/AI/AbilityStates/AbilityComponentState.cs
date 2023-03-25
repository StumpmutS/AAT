using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponentState : AiComponentState, IGameActionInfoGetter
{
    [SerializeField] private List<AbilityGameAction> abilityGameActions;
    [SerializeField] private float duration;

    private IMoveSystem _moveSystem;

    private bool _abilityUsed;
    protected StumpTarget _target;

    public event Action OnStateFinished = delegate { };

    protected override void OnSpawnSuccess()
    {
        _moveSystem = Container.GetComponent<IMoveSystem>();
    }

    protected override void OnEnter()
    {
        _moveSystem.Stop();
        _moveSystem.Disable();
        ActivateActions();
        StartCoroutine(CoFinishAbility());
    }

    private void ActivateActions()
    {
        GameActionRunner.Instance.PerformActions(abilityGameActions, this);
    }

    public IEnumerable<GameActionInfo> GetInfo()
    {
        return new[]
        {
            new GameActionInfo(Container.Object, Container.GetComponent<SectorReference>().Sector,
                Container.GetComponent<TransformContainer>().ToChain(), _target)
        };
    }

    protected override void Tick() { }

    private IEnumerator CoFinishAbility()
    {
        yield return new WaitForSeconds(duration);
        _stateMachine.Exit(this);
    }

    public override void OnExit()
    {
        _moveSystem.Enable();
        OnStateFinished.Invoke();
    }

    public void SetTarget(StumpTarget target)
    {
        _target = target;
    }
}
