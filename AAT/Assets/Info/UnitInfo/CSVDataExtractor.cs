using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSVDataExtractor
{
    [MenuItem("Utilities/GenerateStats")]
    public static void GenerateStats()
    {
        var csvPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var pathMinusAssets = csvPath.Remove(0, 6);
        
        var allCsvLines = File.ReadAllLines(Application.dataPath + "\\" + pathMinusAssets);
        var enums = allCsvLines[0].Split(',');
        var folderPath = csvPath.Remove(csvPath.LastIndexOf("/"));
        for (int i = 1; i < allCsvLines.Length; i++)
        {
            var splitLine = allCsvLines[i].Split(',');
            var unitStatsPath = folderPath + "/" + splitLine[0] + "Info/Stats/";

            var statContainerSo = ScriptableObject.CreateInstance<BaseUnitStatsData>();
            for (int j = 1; j < splitLine.Length; j++)
            {
                var statSo = ScriptableObject.CreateInstance<UnitStat>();
                statSo.Stat = (EUnitFloatStats)Enum.Parse(typeof(EUnitFloatStats), enums[j]);
                statSo.Value = float.Parse(splitLine[j]);
                statContainerSo.Stats.Add(statSo);
                AssetDatabase.CreateAsset(statSo, unitStatsPath + splitLine[0] + enums[j] + "B.asset");
            }
            AssetDatabase.CreateAsset(statContainerSo, unitStatsPath + splitLine[0] + "StatsData.asset");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
