using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Scripts;

public class SectorDivider : MonoBehaviour
{
    [SerializeField] private SerializableTuple<SectorController, SectorController> sectors;
    public SerializableTuple<SectorController, SectorController> Sectors => sectors;

    public void SetSectors(SerializableTuple<SectorController, SectorController> newSectors)
    {
        sectors = newSectors;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TraverseController>(out var traverseController))
        {
            traverseController.AttemptTraverse();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<TraverseController>(out var traverseController))
        {
            traverseController.AttemptTraverse();
        }
    }
}
