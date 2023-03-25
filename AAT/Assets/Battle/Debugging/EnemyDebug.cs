using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Utility.Scripts;

public class EnemyDebug : MonoBehaviour
{
    [SerializeField] private Image tint;
    
    private void Awake()
    {
        BaseInputManager.OnPPressed += ActivateDebug;
    }

    private void ActivateDebug()
    {
        tint.gameObject.SetActive(true);
        BaseInputManager.OnPPressed -= ActivateDebug;
        BaseInputManager.OnPPressed += DeactivateDebug;
        BaseInputManager.OnLeftClickDown += PlaceUnit;
    }

    private void DeactivateDebug()
    {
        tint.gameObject.SetActive(false);
        BaseInputManager.OnPPressed -= DeactivateDebug;
        BaseInputManager.OnPPressed += ActivateDebug;
        BaseInputManager.OnLeftClickDown -= PlaceUnit;
    }

    private void PlaceUnit()
    {
        throw new NotImplementedException();
        
        /*if (UIHoveredReference.Instance.OverUI()) return;
        
        StumpNetworkRunner.Instance.Runner.Spawn(unit, BaseInputManager.LeftClickPosition, Quaternion.identity, onBeforeSpawned:
            (_, o) =>
            {
                var unit = o.GetComponent<UnitController>();
                //unit.Init(2, SectorFinder.FindSector(BaseInputManager.LeftClickPosition, 5, LayerManager.Instance.GroundLayer), null);
            });*/
    }
}
