using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionRunner : NetworkedSingleton<GameActionRunner>
{
    private HashSet<IntervalEventTimer<GameActionInfo>> _actionEventTimers = new();
    
    public void PerformActions(IEnumerable<StumpGameAction> actions, IGameActionInfoGetter getter)
    {
        foreach (var action in actions)
        {
            foreach (var info in getter.GetInfo())
            { 
                StartCoroutine(DelayActionCoroutine(action, info));
                action.PerformAction(info);
            }
        }
    }
    
    public void StopActions(IEnumerable<StumpGameAction> actions, IGameActionInfoGetter getter)
    {
        foreach (var action in actions)
        {
            foreach (var info in getter.GetInfo())
            { 
                action.StopAction(info);
            }
        }
    }

    private IEnumerator DelayActionCoroutine(StumpGameAction abilityGameAction, GameActionInfo info)
    {
        yield return new WaitForSeconds(abilityGameAction.Delay);
        var timer = new IntervalEventTimer<GameActionInfo>(abilityGameAction.RepeatIntervals, info, abilityGameAction.PerformAction);
        if (abilityGameAction.Repeat)
        {
            _actionEventTimers.Add(timer);
        }
        abilityGameAction.PerformAction(info);
        yield return new WaitForSeconds(abilityGameAction.Duration);
        _actionEventTimers.Remove(timer);
        abilityGameAction.StopAction(info);
    }

    public override void FixedUpdateNetwork()
    {
        foreach (var timer in _actionEventTimers)
        {
            timer.Tick(Runner.DeltaTime);
        }
    }
}