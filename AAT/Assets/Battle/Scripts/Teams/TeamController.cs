using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    [Tooltip("Team numbers start at 1, 0 is no team")] [SerializeField] private int teamNumber;
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
        foreach (var gObject in layersToChange)
        {
            gObject.layer = LayerMask.NameToLayer($"Team{teamNumber}");
        }
    }
}
