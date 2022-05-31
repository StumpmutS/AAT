using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/Passives/Unit Passive Data")]
public class UnitPassiveData : ScriptableObject
{
    public UnitPassiveDataInfo UnitPassiveDataInfo;
}

[Serializable]
public class UnitPassiveDataInfo
{
    public string PassiveName;
    public bool TransportUsage;
    public List<PassiveComponent> PassiveComponents;
}