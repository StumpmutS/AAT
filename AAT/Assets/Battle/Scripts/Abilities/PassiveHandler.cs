using System.Collections.Generic;
using UnityEngine;

public class PassiveHandler : MonoBehaviour
{
    [SerializeField] UnitController unit;
    [SerializeField] private List<UnitPassiveData> unitPassiveData;

    //delicious
    private void Start() => unitPassiveData.ForEach(u => u.UnitPassiveDataInfo.PassiveComponents.ForEach(c => c.ActivateComponent(unit)));
}
