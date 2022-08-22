using System;
using UnityEngine;

[RequireComponent(typeof(OutlineController))]
public class OutlineColorController : MonoBehaviour
{
    [SerializeField] private ColorsData outlineColors;

    private OutlineController _outline;
    private OutlineSelectableController _outlineSelectable;
    private TeamController _team;

    private void Awake()
    {
        _outline = GetComponent<OutlineController>();
        _outlineSelectable = GetComponent<OutlineSelectableController>();
        _outlineSelectable.OnHover += SetOutlineColors;
        _outlineSelectable.OnSelect += SetOutlineColors;
        _team = GetComponent<TeamController>();
    }

    private void SetOutlineColors()
    {
        if (_team == null || _team.Object == null)
        {
            _outline.OutlineColor = outlineColors.Colors[0]; //TODO
            return;
        }
        
        var localTeam = _team.Runner.GetPlayerObject(_team.Runner.LocalPlayer).GetComponent<TeamController>();
        if (TargetHelper.TargetRelation(localTeam, _team, ETargetRelation.Enemy))
        {
            _outline.OutlineColor = outlineColors.Colors[3];
        }
        else if (TargetHelper.TargetRelation(localTeam, _team, ETargetRelation.Owned))
        {
            _outline.OutlineColor = outlineColors.Colors[1];
        }
    }

    private void OnDestroy()
    {
        if (_outlineSelectable == null) return;
        _outlineSelectable.OnHover -= SetOutlineColors;
        _outlineSelectable.OnSelect -= SetOutlineColors;
    }
}
