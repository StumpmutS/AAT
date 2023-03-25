using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class Restriction : ScriptableObject
{
    [SerializeField] private int transformIndex;
    
    public abstract bool CheckRestriction(IEnumerable<GameActionInfo> info);

    protected Transform GetTransform(TransformChain chain)
    {
        return chain.Transforms[transformIndex];
    }
}