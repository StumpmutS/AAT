using Fusion;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Unit Data/Restrictions/Range Restriction")]
public class RangeRestriction : Restriction
{
    [SerializeField] private float range;

    public override bool CheckRestriction(UnitController unit)
    {
        if (!StumpNetworkRunner.Instance.Runner.TryGetInputForPlayer(unit.Object.InputAuthority, out NetworkedInputData input)) return false;
        CollisionDetector.CheckRadius(unit.Runner, unit.Object.InputAuthority, input.LeftClickPosition, .1f, Physics.AllLayers, out var hit);
        var leftClickPos0Y = input.LeftClickPosition;
        leftClickPos0Y.y = 0;
        var unit0Y = unit.transform.position;
        unit0Y.y = 0;
        return Vector3.Distance(unit0Y, leftClickPos0Y) < range;
    }
}