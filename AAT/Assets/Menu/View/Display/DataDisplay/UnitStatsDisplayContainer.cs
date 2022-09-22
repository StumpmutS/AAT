using UnityEngine;

public class UnitStatsDisplayContainer : DataDisplayContainer
{
    [SerializeField] private LayoutDisplay layoutDisplay;
    [SerializeField] private FullImageTextDisplay unitStatDisplayPrefab;

    public override void DisplayData(StumpData data)
    {
        layoutDisplay.Clear();
        if (data == null) return;
        
        var unitData = (UnitData) data;
        var stats = unitData.UnitStatsData.Stats;
        var statIcons = unitData.UnitArtData.UnitStatIcons.StatIcons;

        foreach (var stat in stats)
        {
            var display = Instantiate(unitStatDisplayPrefab);

            if (statIcons.ContainsKey(stat.Stat)) display.SetStylizedImages(statIcons[stat.Stat]);
            display.SetText(stat.Value.ToString("F0"));
            
            layoutDisplay.Add(display.transform);
        }
    }
}