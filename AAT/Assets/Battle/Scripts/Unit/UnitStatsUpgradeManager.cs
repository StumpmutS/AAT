using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatsUpgradeManager : MonoBehaviour
{
    [SerializeField] private UnitStatsData baseUnitStatsData;

    private UnitStatsData currentUnitStatsData;
    public UnitStatsData CurrentUnitStatsData => currentUnitStatsData;

    private void Awake()
    {
        currentUnitStatsData = baseUnitStatsData;
    }

    //upgrade methods
}
