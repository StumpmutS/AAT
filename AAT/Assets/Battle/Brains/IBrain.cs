public interface IBrain<T> where T : TransitionBlackboard
{
    public T GetBlackboard();
    
    public ComponentState<T> AddOrGetState(ComponentState<T> componentState);
}