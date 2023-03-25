public class PatrolButtonSender : UserActionSender<MovementDataContainer>
{
    protected override ESelectionType SelectionType() => ESelectionType.Group;

    protected override ESubCategory SubCategory() => ESubCategory.Movement;

    protected override void UpdateDisplay(UserActionDisplay display, MovementDataContainer abilityDataContainer)
    {
        //display.Overlay.SetStylizedImages(imageOverlay);
    }
}