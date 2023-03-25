using Fusion;

public class IdleComponentState : AiComponentState
{
    private IMoveSystem _moveSystem;

    protected override void OnSpawnSuccess()
    {
        _moveSystem = Container.GetComponent<IMoveSystem>();
    }

    protected override void OnEnter()
    {
        if (!Runner.IsServer) return;
        
        _moveSystem.Stop();
    }

    protected override void Tick() { }

    public override void OnExit() { }
}
