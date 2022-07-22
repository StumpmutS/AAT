using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class TeamController : NetworkBehaviour
{
    [Networked] private int teamNumber { get; set; }
    
    [SerializeField] private List<GameObject> layersToChange;
    
    private void Start()
    {
        SetLayers();
    }
    
    public int GetTeamNumber() => teamNumber;
    
    public void SetTeamNumber(int number)
    {
        teamNumber = number;
        SetLayers();
    }

    private void SetLayers()
    {
        if (Object == null) return;
        
        foreach (var gObject in layersToChange)
        {
            gObject.layer = LayerMask.NameToLayer($"Team{teamNumber}");
        }
    }
}
