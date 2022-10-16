using System;

public class AbilityUserActionSender : UserActionSender<AbilityDataContainer>
{
    protected override ESelectionType SelectionType() => ESelectionType.Group;

    protected override ESubCategory SubCategory() => ESubCategory.Ability;

    protected override void UpdateDisplay(UserActionDisplay display, AbilityDataContainer abilityDataContainer)
    {
        display.Overlay.SetStylizedImages(imageOverlay);
    }
}