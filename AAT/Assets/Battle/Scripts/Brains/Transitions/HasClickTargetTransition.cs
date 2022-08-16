using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transitions/Has Click Target")]
public class HasClickTargetTransition : Transition
{
    public override bool Decision(UnitController unit)
    {
        if (!unit.NetworkSelected) return false;
        var hit = unit.Runner.GetPlayerObject(unit.Object.InputAuthority).GetComponent<Player>().NetworkInputHit;
        return hit.GameObject != null;
    }
}
