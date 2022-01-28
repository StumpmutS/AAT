using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPointController : MonoBehaviour
{
    [SerializeField] private UnitDeathController deathController;
    [SerializeField] private bool startEnd;
    public bool StartEnd => startEnd;
    
    public MountablePointLinkController Link { get; private set; }
    
    private void Awake()
    {
        if (deathController != null) deathController.OnUnitDeath += DestroyPoint;
    }

    public void Setup(MountablePointLinkController link, bool start)
    {
        if (Link is { }) return;
        startEnd = start;
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
