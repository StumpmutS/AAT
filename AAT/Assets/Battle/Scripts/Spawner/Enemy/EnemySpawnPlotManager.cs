using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPlotManager : MonoBehaviour
{
    [SerializeField] private float waveSetupTime;
    [SerializeField] private List<EnemySpawnerPlotController> spawnerPlots;
    [Tooltip("Each wave must correspond with spawner")]
    [SerializeField] private List<UnitSpawnDataListList> wavesUnitSpawnData;
    public static List<EnemySpawnerPlotController> SpawnerPlots => instance.spawnerPlots;

    private static List<EnemySpawnerPlotController> activeSpawnerPlots = new List<EnemySpawnerPlotController>();

    public static event Action OnNextWave = delegate { };

    private static EnemySpawnPlotManager instance;
    private static int currentWaveIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetupWave(currentWaveIndex);
    }

    public static void NextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex < instance.wavesUnitSpawnData.Count)
            SetupWave(currentWaveIndex);
        OnNextWave.Invoke();
    }

    private static void SetupWave(int waveIndex)
    {
        instance.StartCoroutine(SetupWaveCoroutine(waveIndex));
    }

    private static IEnumerator SetupWaveCoroutine(int waveIndex)
    {
        yield return new WaitForSeconds(instance.waveSetupTime);

        activeSpawnerPlots.Clear();

        for (int i = 0; i < instance.spawnerPlots.Count; i++)
        {
            instance.spawnerPlots[i].SetupSpawner(instance.wavesUnitSpawnData[waveIndex].UnitSpawnDataList[i], instance.wavesUnitSpawnData[waveIndex].UnitPatrolPointLists[i].Vector3s);
            activeSpawnerPlots.Add(instance.spawnerPlots[i]);
        }
    }
}
