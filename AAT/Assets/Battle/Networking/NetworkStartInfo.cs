using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkStartInfo : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<SectorController, SpawnPlotController> startSpawnersBySector;
    public SerializableDictionary<SectorController, SpawnPlotController> StartSpawnersBySector => startSpawnersBySector;
    
    public static NetworkStartInfo Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
