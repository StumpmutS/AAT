using System;

public class TransitionData<T> where T : TransitionBlackboard
{
    public Func<T, bool> MainCondition { get; }
    public ComponentState<T> To { get; }

    public TransitionData(ComponentState<T> to, Func<T, bool> mainCondition)
    {
        To = to;
        MainCondition = mainCondition;
    }
}