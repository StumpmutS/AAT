using UnityEngine;
using UnityEngine.Serialization;

public class LinkPointController : MonoBehaviour
{
    [SerializeField] private UnitDeathController deathController;
    [FormerlySerializedAs("startEnd")] [SerializeField] private bool start;
    public bool Start => start;
    
    public MountablePointLinkController Link { get; private set; }
    
    private void Awake()
    {
        if (deathController != null) deathController.OnUnitDeath += DestroyPoint;
    }

    public void Setup(MountablePointLinkController link, bool start)
    {
        if (Link is { }) return;
        this.start = start;
        Link = link;
    }

    public void ResetLink(MountablePointLinkController link)
    {
        Link = link;
    }

    private void DestroyPoint()
    {
        //TODO:
    }
}
