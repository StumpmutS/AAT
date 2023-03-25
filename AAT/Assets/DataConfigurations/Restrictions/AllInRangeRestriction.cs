using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Restrictions/All In Range Restriction")]
public class AllInRangeRestriction : Restriction
{
    [SerializeField] private float range;

    public override bool CheckRestriction(IEnumerable<GameActionInfo> info)
    {
        return info.All(i => Vector3.Distance(GetTransform(i.TransformChain).position, i.Target.Point) <= range);
    }
}