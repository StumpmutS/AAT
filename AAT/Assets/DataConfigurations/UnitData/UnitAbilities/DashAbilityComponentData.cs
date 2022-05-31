using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Dash Ability Component")]
public class DashAbilityComponentData : AbilityComponent, ISerializationCallbackReceiver
{
    [SerializeField] private Vector3 dashDistance;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float durationPadding;

    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        unit.GetComponent<DashController>().Dash(dashDistance, dashSpeed);
    }

    public void OnBeforeSerialize()
    {
        ComponentDuration = dashDistance.magnitude / dashSpeed + durationPadding;
    }

    public void OnAfterDeserialize()
    {
        ComponentDuration = dashDistance.magnitude / dashSpeed + durationPadding;
    }
}