using System.Collections.Generic;
using UnityEngine;

public class BaseSpawnPlotManager : MonoBehaviour
{
    [SerializeField] protected List<SpawnPlotController> spawnerPlots;

    public List<SpawnPlotController> SpawnerPlots => spawnerPlots;
}
