public class PlayerOverriddenBrainComponentState : AiComponentState
{
    private IMoveSystem _moveSystem;
    
    protected override void OnSpawnSuccess()
    {
        _moveSystem = Container.GetComponent<IMoveSystem>();
    }

    protected override void OnEnter()
    {
        var target = Player.RightClickTarget;
        _moveSystem.SetTarget(target);
    }

    protected override void Tick() { }

    public override void OnExit() { }
}