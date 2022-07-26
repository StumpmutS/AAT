using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class BaseSpawnerController : SimulationBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    
    private UnitStatsModifierManager _unitStatsModifierManager;

    protected int _spawnGroupsAmount;
    private int _unitsPerGroupAmount;
    private float _spawnTime;
    private float _respawnTime;
    private int _maxSpawnLocationUse;
    private Vector3 _spawnerOffset;
    private SelectableController _spawnerVisuals;
    protected NetworkPrefabRef _unitPrefab;
    private UnitGroupController _unitGroupPrefab;
    protected GameObject _upgradesUIContainer;
    private List<UnitStatsUpgradeData> _upgrades;
    private int _currentUpgradeIndex;
    protected SectorController _sectorController;

    public SelectableController CurrentSpawnerVisualsSelectable { get; private set; }

    private int _currentSpawningCount;
    private Dictionary<Transform, int> _spawnPointActiveGroups = new();

    private List<int> _queuedGroupIndex = new();
    private List<int> _queuedUnitsPerGroup = new();

    protected List<UnitGroupController> _activeUnitGroups = new();
    protected Dictionary<int, int> _unitGroupNumbers = new();
    private Dictionary<int, IEnumerator> _unitGroupCoroutines = new();

    public event Action<BaseSpawnerController> OnSpawnerSelect = delegate { };
    public event Action<BaseUnitStatsData> OnModifyStats = delegate { };

    private void Awake()
    {
        if (Object.HasInputAuthority) SpawnerManager.Instance.AddSpawnerPlot(this);
        _unitStatsModifierManager = GetComponent<UnitStatsModifierManager>();
        foreach (var spawnPoint in spawnPoints)
        {
            _spawnPointActiveGroups.Add(spawnPoint, -1);
        }
    }

    public void Setup(UnitSpawnData unitSpawnData, SectorController sector, GameObject upgradesUIContainer = null)
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
        _sectorController = sector;

        _unitStatsModifierManager.Init(unitSpawnData.baseUnitStatsData);

        for (int i = 0; i < _spawnGroupsAmount; i++)
        {
            AddEmptyUnitGroup();
        }
        
        transform.LookAt(new Vector3(0, transform.position.y, 0));
        InititializeVisuals();
        InitiliazeSpawning();
        CurrentSpawnerVisualsSelectable.CallSelectOverrideUICheck();
    }

    #region Visuals
    private void InititializeVisuals()
    {
        var instantiatedSpawnerVisuals = Instantiate(_spawnerVisuals, gameObject.transform);
        instantiatedSpawnerVisuals.transform.position += _spawnerOffset;
        CurrentSpawnerVisualsSelectable = instantiatedSpawnerVisuals;
        instantiatedSpawnerVisuals.OnSelect += SelectHandler;
        instantiatedSpawnerVisuals.OnDeselect += DeselectHandler;
    }

    protected virtual void SelectHandler()
    {
        OnSpawnerSelect.Invoke(this);
    }

    protected virtual void DeselectHandler() { }
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
        if (_currentSpawningCount < _maxSpawnLocationUse)
        {
            _currentSpawningCount++;
            SpawnUnitGroup(spawnTime, groupIndex, unitsPerGroup);
        }
        else
        {
            _queuedGroupIndex.Add(groupIndex);
            _queuedUnitsPerGroup.Add(unitsPerGroup);
        }
    }

    private void SpawnUnitGroup(float spawnTime, int groupIndex = -1, int unitsPerGroup = -1)
    {
        StartCoroutine(SpawnUnitGroupCoroutine(spawnTime, groupIndex, unitsPerGroup));
    }

    private IEnumerator SpawnUnitGroupCoroutine(float spawnTime, int groupIndex, int unitsPerGroup)
    {
        int spawnPointIndex = GetFirstInactiveSpawnerIndex();
        if (groupIndex < 0)
        {
            groupIndex = GetEmptyGroup();
        }
        if (unitsPerGroup < 0)
        {
            unitsPerGroup = _unitsPerGroupAmount;
        }

        _unitGroupNumbers[groupIndex] = unitsPerGroup;

        _spawnPointActiveGroups[spawnPoints[spawnPointIndex]] = groupIndex;

        IEnumerator spawnUnitsCoroutine = SpawnUnitsCoroutine(spawnTime, groupIndex);
        _unitGroupCoroutines[groupIndex] = spawnUnitsCoroutine;
        UnitGroupController unitGroup = _activeUnitGroups[groupIndex];
        unitGroup.transform.position = spawnPoints[spawnPointIndex].position;
        
        StartCoroutine(spawnUnitsCoroutine);
        yield return new WaitForSeconds(spawnTime);
        
        if (_unitGroupNumbers[groupIndex] != unitsPerGroup && _unitGroupNumbers[groupIndex] >= _unitsPerGroupAmount || !_unitGroupCoroutines.ContainsKey(groupIndex)) yield break;
        
        unitGroup.Setup(ActiveUnitDeathHandler, this, groupIndex);
        _spawnPointActiveGroups[spawnPoints[spawnPointIndex]] = -1;
        _currentSpawningCount--;

        //if there is a unit group in queue use the first inactive spawn point to spawn the queued unit group
        if (_queuedGroupIndex.Count < 1) yield break;
        
        int inactiveSpawnerIndex = GetFirstInactiveSpawnerIndex();
        
        if (inactiveSpawnerIndex < 0) yield break;
        
        int queuedIndex = _queuedGroupIndex[0];
        int queuedUnitsCount = _queuedUnitsPerGroup[0];
        _queuedGroupIndex.RemoveAt(0);
        _queuedUnitsPerGroup.RemoveAt(0);
        _currentSpawningCount++;
        SpawnUnitGroup(spawnTime, queuedIndex, queuedUnitsCount);
    }

    protected virtual IEnumerator SpawnUnitsCoroutine(float spawnTime, int groupIndex)
    {
        yield return new WaitForSeconds(spawnTime);
        for (int i = 0; i < _unitGroupNumbers[groupIndex]; i++)
        {
            UnitController instantiatedUnit = Runner.Spawn(_unitPrefab, _activeUnitGroups[groupIndex].transform.position, Quaternion.identity).GetComponent<UnitController>();
            _activeUnitGroups[groupIndex].AddUnit(instantiatedUnit);
            _sectorController.ModifySectorPower(instantiatedUnit.Stats.CalculatePower());
            instantiatedUnit.SetSector(_sectorController);
            for (int j = 0; j < _currentUpgradeIndex; j++)
            {
                instantiatedUnit.ModifyStats(_upgrades[j].baseUnitStatsDataInfo);
            }
        }
    }
    #endregion

    #region Respawning
    protected abstract void ActiveUnitDeathHandler(int groupIndex);

    protected void RespawnUnitGroup(int groupIndex)
    {
        if (CheckQueue(groupIndex, out int index))
        {
            _queuedUnitsPerGroup[index]++;
            return;
        }

        int spawnPointIndex = CheckSpawning(groupIndex);

        if (spawnPointIndex > -1)
        {
            StopCoroutine(_unitGroupCoroutines[groupIndex]);
            _unitGroupCoroutines.Remove(groupIndex);
            _spawnPointActiveGroups[spawnPoints[spawnPointIndex]] = -1;
            _currentSpawningCount--;
        }
        QueueUnitGroup(_respawnTime, groupIndex);
    }

    protected void RespawnUnit(int groupIndex)
    {
        if (CheckQueue(groupIndex, out int index))
        {
            _queuedUnitsPerGroup[index]++;
        }
        else if (CheckSpawning(groupIndex) > -1)
        {
            _unitGroupNumbers[groupIndex]++;
        }
        else
        {
            QueueUnitGroup(_respawnTime, groupIndex, 1);
        }
    }
    #endregion

    #region StatsModification

    public void ModifyUnitGroupStats()
    {
        if (_currentUpgradeIndex >= _upgrades.Count) return;

        BaseUnitStatsData upgradeStats = _upgrades[_currentUpgradeIndex].baseUnitStatsDataInfo;

        _unitStatsModifierManager.ModifyStats(upgradeStats);
        OnModifyStats.Invoke(upgradeStats);
        _currentUpgradeIndex++;
    }
    #endregion

    #region Helper Methods
    private int GetFirstInactiveSpawnerIndex()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (_spawnPointActiveGroups[spawnPoints[i]] < 0)
            {
                return i;
            }
        }
        Debug.LogError("No available spawners");
        return -1;
    }

    private void AddEmptyUnitGroup()
    {
        UnitGroupController unitGroup = Instantiate(_unitGroupPrefab, Vector3.zero, Quaternion.identity);
        _activeUnitGroups.Add(unitGroup);
    }

    private int GetEmptyGroup()
    {
        for (int i = 0; i < _activeUnitGroups.Count; i++)
        {
            if (_activeUnitGroups[i].Units.Count == 0 && i >= _unitGroupCoroutines.Count)
            {
                return i;
            }
        }
        return -1;
    }

    private bool CheckQueue(int groupIndex, out int index)
    {
        for (int i = 0; i < _queuedGroupIndex.Count; i++)
        {
            if (_queuedGroupIndex[i] == groupIndex)
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
            if (_spawnPointActiveGroups[spawnPoints[i]] == groupIndex)
            {
                return i;
            }
        }
        return -1;
    }
    #endregion
}
