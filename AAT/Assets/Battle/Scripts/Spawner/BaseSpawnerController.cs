using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawnerController : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private UnitStatsModifierManager unitStatsModifierManager;

    protected int _spawnGroupsAmount;
    private int _unitsPerGroupAmount;
    private float _spawnTime;
    private float _respawnTime;
    private int _maxSpawnLocationUse;
    private Vector3 _spawnerOffset;
    private GameObject _spawnerVisuals;
    private UnitController _unitPrefab;
    private UnitGroupController _unitGroupPrefab;
    protected RectTransform _upgradesUIContainer;
    private List<UnitStatsUpgradeData> _upgrades;
    private int _currentUpgradeIndex;

    private EntityController _currentSpawnerVisualsEntity;
    public EntityController CurrentSpawnerVisualsEntity => _currentSpawnerVisualsEntity;

    private int currentSpawningCount;
    private Dictionary<Transform, int> spawnPointActiveGroups = new Dictionary<Transform, int>();

    private List<int> queuedGroupIndex = new List<int>();
    private List<int> queuedUnitsperGroup = new List<int>();

    protected List<UnitGroupController> activeUnitGroups = new List<UnitGroupController>();
    private Dictionary<int, int> unitGroupNumbers = new Dictionary<int, int>();
    private Dictionary<int, IEnumerator> unitGroupCoroutines = new Dictionary<int, IEnumerator>();

    public event Action<BaseSpawnerController> OnSpawnerSelect = delegate { };

    private void Awake()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPointActiveGroups.Add(spawnPoint, -1);
        }
    }

    public void Setup(UnitSpawnData unitSpawnData, RectTransform upgradesUIContainer = null)
    {
        _spawnGroupsAmount = unitSpawnData.SpawnGroupsAmount;
        _unitsPerGroupAmount = unitSpawnData.UnitsPerGroupAmount;
        _spawnTime = unitSpawnData.SpawnTime;
        _respawnTime = unitSpawnData.RespawnTime;
        _maxSpawnLocationUse = unitSpawnData.MaxSpawnLocationUse;
        _spawnerVisuals = unitSpawnData.SpawnerVisuals;
        _spawnerOffset = unitSpawnData.SpawnerOffset;
        _unitPrefab = unitSpawnData.UnitPrefab;
        _unitGroupPrefab = unitSpawnData.UnitGroupPrefab;
        _upgradesUIContainer = upgradesUIContainer;
        _upgrades = unitSpawnData.UnitStatsUpgradeData;

        unitStatsModifierManager.Setup(unitSpawnData.UnitStatsData);

        for (int i = 0; i < _spawnGroupsAmount; i++)
        {
            AddEmptyUnitGroup();
        }
        InititializeVisuals();
        InitiliazeSpawning();
    }

    #region Visuals
    private void InititializeVisuals()
    {
        GameObject instantiatedSpawnerVisuals = Instantiate(_spawnerVisuals, gameObject.transform);
        instantiatedSpawnerVisuals.transform.position += _spawnerOffset;
        EntityController spawnerVisualsEntity = instantiatedSpawnerVisuals.GetComponent<EntityController>();
        _currentSpawnerVisualsEntity = spawnerVisualsEntity;
        spawnerVisualsEntity.OnSelect += Select;
        spawnerVisualsEntity.OnDeselect += Deselect;
    }

    protected virtual void Select()
    {
        OnSpawnerSelect.Invoke(this);
    }

    protected virtual void Deselect() { }
    #endregion

    #region Spawning
    private void InitiliazeSpawning()
    {
        for (int i = 0; i < _spawnGroupsAmount; i++)
        {
            QueueUnitGroup(_spawnTime);
        }
    }

    private void QueueUnitGroup(float spawnTime, int groupIndex = -1, int unitsPerGroup = -1)
    {
        if (currentSpawningCount < _maxSpawnLocationUse)
        {
            currentSpawningCount++;
            SpawnUnitGroup(spawnTime, groupIndex, unitsPerGroup);
        }
        else
        {
            queuedGroupIndex.Add(groupIndex);
            queuedUnitsperGroup.Add(unitsPerGroup);
        }
    }

    private void SpawnUnitGroup(float spawnTime, int groupIndex = -1, int unitsPerGroup = -1)
    {
        StartCoroutine(SpawnUnitGroupCoroutine(spawnTime, groupIndex, unitsPerGroup));
    }

    private IEnumerator SpawnUnitGroupCoroutine(float spawnTime, int groupIndex, int unitsPerGroup)
    {
        int spawnPointIndex = GetFirstInactiveSpawnerIndex();
        if (groupIndex <= -1)
        {
            groupIndex = GetEmptyGroup();
        }
        if (unitsPerGroup <= -1)
        {
            unitsPerGroup = _unitsPerGroupAmount;
        }

        unitGroupNumbers[groupIndex] = unitsPerGroup;

        spawnPointActiveGroups[spawnPoints[spawnPointIndex]] = groupIndex;

        IEnumerator spawnUnitsCoroutine = SpawnUnitsCoroutine(spawnTime, groupIndex);
        unitGroupCoroutines[groupIndex] = spawnUnitsCoroutine;
        StartCoroutine(spawnUnitsCoroutine);

        UnitGroupController unitGroup = activeUnitGroups[groupIndex];
        unitGroup.transform.position = spawnPoints[spawnPointIndex].position;

        yield return new WaitForSeconds(spawnTime);

        if (unitGroupNumbers[groupIndex] == unitsPerGroup)
        {
            unitGroup.Setup(ActiveUnitDeathHandler, this, groupIndex);

            spawnPointActiveGroups[spawnPoints[spawnPointIndex]] = -1;
            currentSpawningCount--;

            //if there is a unit group in queue use the first inactive spawn point to spawn the queued unit group
            if (queuedGroupIndex.Count > 0)
            {
                int inactiveSpawnerIndex = GetFirstInactiveSpawnerIndex();
                if (inactiveSpawnerIndex > -1)
                {
                    int queuedIndex = queuedGroupIndex[queuedGroupIndex.Count - 1];
                    int queuedUnitsCount = queuedUnitsperGroup[queuedUnitsperGroup.Count - 1];
                    queuedGroupIndex.RemoveAt(queuedGroupIndex.Count - 1);
                    queuedUnitsperGroup.RemoveAt(queuedUnitsperGroup.Count - 1);
                    currentSpawningCount++;
                    SpawnUnitGroup(spawnTime, queuedIndex, queuedUnitsCount);
                }
            }
        }
    }

    private IEnumerator SpawnUnitsCoroutine(float spawnTime, int groupIndex)
    {
        yield return new WaitForSeconds(spawnTime);
        for (int i = 0; i < unitGroupNumbers[groupIndex]; i++)
        {
            UnitController instantiatedUnit = Instantiate(_unitPrefab, activeUnitGroups[groupIndex].transform.position, Quaternion.identity);
            activeUnitGroups[groupIndex].AddUnit(instantiatedUnit);
        }
    }
    #endregion

    #region Respawning
    protected abstract void ActiveUnitDeathHandler(int groupIndex);

    protected void RespawnUnitGroup(int groupIndex)
    {
        if (CheckQueue(groupIndex, out int index))
        {
            queuedUnitsperGroup[index]++;
            return;
        }

        int spawnPointIndex = CheckSpawning(groupIndex);

        if (spawnPointIndex > -1)
        {
            StopCoroutine(unitGroupCoroutines[groupIndex]);
            spawnPointActiveGroups[spawnPoints[spawnPointIndex]] = -1;
            currentSpawningCount--;
        }
        QueueUnitGroup(_respawnTime, groupIndex);
    }

    protected void RespawnUnit(int groupIndex)
    {
        if (CheckQueue(groupIndex, out int index))
        {
            queuedUnitsperGroup[index]++;
            return;
        }

        if (CheckSpawning(groupIndex) > -1)
        {
            unitGroupNumbers[groupIndex]++;
        }
        else
        {
            QueueUnitGroup(_respawnTime, groupIndex, 1);
        }
    }
    #endregion

    #region StatsModification
    public event Action<UnitStatsDataInfo> OnModifyStats = delegate { };

    public void ModifyUnitGroupStats()
    {
        if (_currentUpgradeIndex >= _upgrades.Count) return;

        UnitStatsDataInfo unitStatsDataInfo = _upgrades[_currentUpgradeIndex].UnitStatsDataInfo;

        unitStatsModifierManager.ModifyStats(unitStatsDataInfo);
        OnModifyStats.Invoke(unitStatsDataInfo);
        _currentUpgradeIndex++;
    }
    #endregion

    #region Helper Methods
    private int GetFirstInactiveSpawnerIndex()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPointActiveGroups[spawnPoints[i]] < 0)
            {
                return i;
            }
        }
        return -1;
    }

    private void AddEmptyUnitGroup()
    {
        UnitGroupController unitGroup = Instantiate(_unitGroupPrefab, Vector3.zero, Quaternion.identity);
        activeUnitGroups.Add(unitGroup);
    }

    private int GetEmptyGroup()
    {
        for (int i = 0; i < activeUnitGroups.Count; i++)
        {
            if (activeUnitGroups[i].Units.Count == 0 && i >= unitGroupCoroutines.Count)
            {
                return i;
            }
        }
        return -1;
    }

    private bool CheckQueue(int groupIndex, out int index)
    {
        for (int i = 0; i < queuedGroupIndex.Count; i++)
        {
            if (queuedGroupIndex[i] == groupIndex)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    private int CheckSpawning(int groupIndex)
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPointActiveGroups[spawnPoints[i]] == groupIndex)
            {
                return i;
            }
        }
        return -1;
    }
    #endregion
}
