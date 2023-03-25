using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Utility.Scripts;

[RequireComponent(typeof(SectorController))]
public class SectorBarController : MonoBehaviour
{
    [SerializeField] private ColorsData relationColors;
    [SerializeField] private float setupBarSpeed;
    [SerializeField] private Image image;
    [SerializeField] private SpringController spring;
    [SerializeField] private float springNudgeSpeed;

    private SectorController _sector;
    private PlayerRef _cachedLocal;
    private TeamController _cachedTeam;
    private float _prevPercent;
    private float _currentPercent;
    private int _highestTeamByPercent;
    private static readonly int Percentage = Shader.PropertyToID("_Percentage");

    private void Awake()
    {
        _sector = GetComponent<SectorController>();
        _sector.OnPercentLeaderTeamChanged += HandlePercentLeaderTeamChanged;
    }

    private void Start()
    {
        if (image.material != null) image.material = Instantiate(image.material);
    }

    private void Update()
    {
        if (_sector.Object == null || _sector.TeamCapturePercentages.Count < 1) return;

        var speed = _sector.TeamCapturePercentages.Max(kvp => kvp.Value) > 99.99f ? setupBarSpeed : _sector.CaptureSpeed;

        var targetPercent = _sector.TeamPowers.MaxKeyByValue() == _highestTeamByPercent ? 100 : 0;
        _currentPercent = Mathf.MoveTowards(_currentPercent, targetPercent, speed * _sector.Runner.DeltaTime);
        image.material.SetFloat(Percentage, Mathf.Abs(_currentPercent));

        if (_currentPercent > 99.99f && _prevPercent < 99.99f) FinishCapture();

        _prevPercent = _currentPercent;
    }

    private void FinishCapture()
    {
        spring.Nudge(springNudgeSpeed);
    }

    private void HandlePercentLeaderTeamChanged(int team)
    {
        _highestTeamByPercent = team;
        
        if (_sector.Object != null && _cachedLocal != _sector.Runner.LocalPlayer)
        {
            UpdateLocalPlayer();
        }
        var leadingTeam = _sector.Runner.GetPlayerObject(TeamManager.Instance.GetPlayerForTeam(team)).GetComponent<TeamController>();
        
        if (TeamRelations.TeamRelation(_cachedTeam, leadingTeam, ETeamRelation.None))
        {
            image.material.color = relationColors.Colors[0];
        }
        if (TeamRelations.TeamRelation(_cachedTeam, leadingTeam, ETeamRelation.Owned))
        {
            image.material.color = relationColors.Colors[1];
        }
        if (TeamRelations.TeamRelation(_cachedTeam, leadingTeam, ETeamRelation.Ally))
        {
            image.material.color = relationColors.Colors[2];
        }
        if (TeamRelations.TeamRelation(_cachedTeam, leadingTeam, ETeamRelation.Enemy))
        {
            image.material.color = relationColors.Colors[3];
        }
    }

    private void UpdateLocalPlayer()
    {
        if (_sector.Object == null) return;
        
        _cachedLocal = _sector.Runner.LocalPlayer;
        _cachedTeam = _sector.Runner.GetPlayerObject(_cachedLocal).GetComponent<TeamController>();
    }
}
