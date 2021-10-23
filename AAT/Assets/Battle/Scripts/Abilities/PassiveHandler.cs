using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveHandler : MonoBehaviour
{
    [SerializeField] UnitController unit;
    [SerializeField] private List<UnitPassiveData> unitPassiveData;

    private void Start()
    {
        //delicious
        unitPassiveData.ForEach(u => u.UnitPassiveDataInfo.PassiveComponents.ForEach(c => c.ActivateComponent(unit)));
    }
}
