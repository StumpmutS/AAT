using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Restrictions/Any In Range Restriction")]
public class AnyInRangeRestriction : Restriction
{
    [SerializeField] private float range;

    public override bool CheckRestriction(IEnumerable<GameActionInfo> info)
    {
        return info.All(i => Vector3.Distance(GetTransform(i.TransformChain).position, i.Target.Point) <= range);
    }
}