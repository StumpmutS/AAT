public class SpawnerController : BaseSpawnerController
{
    protected override void ActiveUnitDeathHandler(int groupIndex)
    {
        if (_activeUnitGroups[groupIndex].Units.Count <= 0)
        {
            RespawnUnitGroup(groupIndex);
        }
        else
        {
            RespawnUnit(groupIndex);
        }
    }

    protected override void SelectHandler()
    {
        base.SelectHandler();
        foreach (var unitGroup in _activeUnitGroups)
        {
            unitGroup.SelectGroup();
        }
        _upgradesUIContainer.SetActive(true);
    }

    protected override void DeselectHandler()
    {
        foreach (var unitGroup in _activeUnitGroups)
        {
            unitGroup.DeselectGroup();
        }
        _upgradesUIContainer.SetActive(false);
    }
}
