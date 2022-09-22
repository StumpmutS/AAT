using UnityEngine;

[CreateAssetMenu(menuName = "Unit Data/General")]
public class GeneralUnitData : ScriptableObject
{
    [SerializeField] private string unitName;
    public string UnitName => unitName;
    [SerializeField, TextArea] private string unitDescription;
    public string UnitDescription => unitDescription;
    [SerializeField] private EFaction faction;
    public EFaction Faction => faction;
}