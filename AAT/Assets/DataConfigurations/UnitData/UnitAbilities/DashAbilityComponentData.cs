using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

[CreateAssetMenu(menuName = "Unit Data/Abilities/Dash Ability Component")]
public class DashAbilityComponentData : AbilityComponent, ISerializationCallbackReceiver
{
    [SerializeField] private List<Vector3> dashPoints;
    [SerializeField, Min(0)] private float dashLerpSpeed;
    [SerializeField] private float durationPadding = .1f;


    public override void ActivateComponent(UnitController unit, Vector3 point = default)
    {
        var dash = unit.AddOrGetComponent<DashController>();
        dash.Configure(dashPoints, dashLerpSpeed);
    }

    public override void DeactivateComponent(UnitController unit)
    {
        if (unit.TryGetComponent<DashController>(out var dash))
        {
            dash.EndDash();
        }
    }

    public void OnBeforeSerialize()
    {
        SetDuration();
    }

    public void OnAfterDeserialize()
    {
        SetDuration();
    }

    private void SetDuration()
    {
        ComponentDuration = 1 / dashLerpSpeed + durationPadding; 
    }
}