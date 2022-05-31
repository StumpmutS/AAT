using System;
using UnityEngine;

[RequireComponent(typeof(SelectableController))]
public class SpawnerPlotController : MonoBehaviour
{
    [SerializeField] private GameObject upgradesUIContainer;
    [SerializeField] private SectorController sector;
    [SerializeField] private EFaction faction;

    private SelectableController _selectable;
    
    public event Action<SpawnerPlotController, EFaction> OnSpawnerPlotSelect = delegate { };
    public event Action OnSpawnerPlotDeselect = delegate { };

    private void Awake()
    {
        _selectable = GetComponent<SelectableController>();
        _selectable.OnSelect += Select;
        _selectable.OnDeselect += Deselect;
    }

    private void Select() => OnSpawnerPlotSelect.Invoke(this, faction);

    private void Deselect() => OnSpawnerPlotDeselect.Invoke();

    public void SetupSpawner(UnitSpawnData spawnData)
    {
        SpawnerController instantiatedSpawner = Instantiate(spawnData.Spawner, transform.position, transform.rotation);
        SpawnerManager.Instance.AddSpawnerPlot(instantiatedSpawner);
        instantiatedSpawner.Setup(spawnData, sector, upgradesUIContainer);
        gameObject.SetActive(false);
    }

    public void Setup(GameObject upgradesUI, SectorController sector, EFaction faction)
    {
        upgradesUIContainer = upgradesUI;
        this.sector = sector;
        this.faction = faction;
    }
    
    public void CallSelect() => _selectable.CallSelect();

    public void CallDeselect() => _selectable.CallDeselect();
}
