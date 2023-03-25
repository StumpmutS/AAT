public class SpawnPlotUserActionSender : UserActionSender<SpawnPlotActionCreator>
{
    protected override ESelectionType SelectionType() => ESelectionType.SpawnPlot;

    protected override ESubCategory SubCategory() => ESubCategory.Spawning;

    protected override void UpdateDisplay(UserActionDisplay display, SpawnPlotActionCreator abilityDataContainer)
    {
        //display.Overlay.SetStylizedImages(imageOverlay);
    }
}