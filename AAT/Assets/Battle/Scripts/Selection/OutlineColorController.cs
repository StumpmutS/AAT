using UnityEngine;

[RequireComponent(typeof(Outline))]
public class OutlineColorController : MonoBehaviour
{
    [SerializeField] private ColorsData outlineColors;

    private Outline _outline;
    private OutlineSelectableController _outlineSelectable;
    private TeamController _team;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outlineSelectable = GetComponent<OutlineSelectableController>();
        _outlineSelectable.OnHover += SetOutlineColors;
        _team = GetComponent<TeamController>();
    }

    private void SetOutlineColors()
    {
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
}
