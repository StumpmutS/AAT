using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class StumpGameAction : ScriptableObject
{
    [SerializeField] private List<Restriction> restrictions;
    public List<Restriction> Restrictions => restrictions;
    [SerializeField] private float delay;
    public float Delay => delay;
    [SerializeField] private float duration;
    public float Duration => duration;
    public bool Repeat;
    [ShowIf(nameof(Repeat), true)]
    public float RepeatIntervals;
    [SerializeField] private int transformIndex;

    public abstract void PerformAction(GameActionInfo info);
    public virtual void StopAction(GameActionInfo info) { }

    protected Transform GetTransform(TransformChain chain)
    {
        return chain.Transforms[transformIndex];
    }
}

public class GameActionInfo
{
    public NetworkObject MainCaller; //group
    public SectorController Sector;
    public TransformChain TransformChain; //units
    public StumpTarget Target;

    public GameActionInfo(NetworkObject mainCaller, SectorController sector, TransformChain transformChain, StumpTarget target)
    {
        MainCaller = mainCaller;
        Sector = sector;
        TransformChain = transformChain;
        Target = target;
    }
}