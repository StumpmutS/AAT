using Fusion;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SectorController))]
public class SectorProgressListener : MonoBehaviour
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
    private float _targetPercent;
    private float _prevSpeed;
    private static readonly int Percentage = Shader.PropertyToID("_Percentage");

    private void Awake()
    {
        _sector = GetComponent<SectorController>();
        _sector.OnSectorCaptureChanged += HandleSectorCaptureChanged;
    }

    private void Start()
    {
        if (image.material != null) image.material = Instantiate(image.material);
    }

    private void Update()
    {
        var speed = _sector.CaptureSpeed;
        if (_targetPercent > 99.999f && (_prevPercent < .1f || _prevSpeed > speed)) speed = setupBarSpeed;
        _prevSpeed = speed;
        
        var newPercent = Mathf.MoveTowards(_prevPercent, _targetPercent, Time.deltaTime * speed);
        if (newPercent > 99.999f && _prevPercent < 99.999f) FinishCapture();
        _prevPercent = newPercent;
        image.material.SetFloat(Percentage, newPercent / 100f);
    }

    private void FinishCapture()
    {
        spring.Nudge(springNudgeSpeed);
    }

    private void HandleSectorCaptureChanged(PlayerRef player, int amount)
    {
        if (player == default) return;
        
        if (_sector.Object != null && _cachedLocal != _sector.Runner.LocalPlayer)
        {
            UpdateLocalPlayer();
        }
        var ownerTeam = _sector.Runner.GetPlayerObject(player).GetComponent<TeamController>();

        _targetPercent = amount;
        
        if (TeamRelations.TeamRelation(_cachedTeam, ownerTeam, ETeamRelation.None))
        {
            image.material.color = relationColors.Colors[0];
        }
        if (TeamRelations.TeamRelation(_cachedTeam, ownerTeam, ETeamRelation.Owned))
        {
            image.material.color = relationColors.Colors[1];
        }
        if (TeamRelations.TeamRelation(_cachedTeam, ownerTeam, ETeamRelation.Ally))
        {
            image.material.color = relationColors.Colors[2];
        }
        if (TeamRelations.TeamRelation(_cachedTeam, ownerTeam, ETeamRelation.Enemy))
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
