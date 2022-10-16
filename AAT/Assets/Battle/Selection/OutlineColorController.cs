using System;
using UnityEngine;

[RequireComponent(typeof(OutlineController), typeof(SelectableOutlineController), typeof(TeamController))]
public class OutlineColorController : MonoBehaviour
{
    [SerializeField] private ColorsData outlineColors;

    private OutlineController _outline;
    private SelectableOutlineController selectableOutline;
    private TeamController _team;

    private void Awake()
    {
        _outline = GetComponent<OutlineController>();
        selectableOutline = GetComponent<SelectableOutlineController>();
        selectableOutline.OnOutline += SetOutlineColors;
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
        if (TeamRelations.TeamRelation(localTeam, _team, ETeamRelation.Enemy))
        {
            _outline.OutlineColor = outlineColors.Colors[3];
        }
        else if (TeamRelations.TeamRelation(localTeam, _team, ETeamRelation.Owned))
        {
            _outline.OutlineColor = outlineColors.Colors[1];
        }
    }

    private void OnDestroy()
    {
        if (selectableOutline == null) return;
        selectableOutline.OnOutline -= SetOutlineColors;
    }
}
