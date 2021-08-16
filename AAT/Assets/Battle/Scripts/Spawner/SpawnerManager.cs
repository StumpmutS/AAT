using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private SpawnPlotManager spawnPlotManager;

    private static List<SpawnerController> spawners = new List<SpawnerController>();

    private static int _nextIndex;

    private void Awake()
    {
        for (int i = 0; i < spawnPlotManager.SpawnerPlots.Count; i++)
        {
            spawners.Add(null);
        }
    }

    private void Start()
    {
        InputManager.OnNumberKey1 += SelectSpawnerByIndex;
        InputManager.OnNumberKey2 += SelectSpawnerByIndex;
        InputManager.OnNumberKey3 += SelectSpawnerByIndex;
        InputManager.OnNumberKey4 += SelectSpawnerByIndex;
        InputManager.OnNumberKey5 += SelectSpawnerByIndex;
        InputManager.OnNumberKey6 += SelectSpawnerByIndex;
    }

    public static void AddSpawnerPlot(SpawnerController spawner)
    {
        spawners[+_nextIndex] = spawner;
    }

    public static void SetNextSpawnerPlotIndex(int index)
    {
        _nextIndex = index;
    }

    private void SelectSpawnerByIndex(int keyPressed)
    {
        foreach (var spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.CurrentSpawnerVisualsEntity.Deselect();
            }
        }
        if (spawners[keyPressed - 1] != null)
        {
            spawners[keyPressed - 1].CurrentSpawnerVisualsEntity.Select();
        }
    }
}
