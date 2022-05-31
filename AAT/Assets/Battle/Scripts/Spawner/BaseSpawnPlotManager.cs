using System.Collections.Generic;
using UnityEngine;

public class BaseSpawnPlotManager : MonoBehaviour
{
    [SerializeField] protected List<SpawnerPlotController> spawnerPlots;

    public List<SpawnerPlotController> SpawnerPlots => spawnerPlots;
}
