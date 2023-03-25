public class UpgradeUserActionSender : UserActionSender<SpawnerUpgradeManager>
{
    protected override ESelectionType SelectionType() => ESelectionType.Spawner;

    protected override ESubCategory SubCategory() => ESubCategory.Upgrades;

    protected override void UpdateDisplay(UserActionDisplay display, SpawnerUpgradeManager spawnerUpgradeManager)
    {
        //display.Overlay.SetStylizedImages(imageOverlay);
    }
}