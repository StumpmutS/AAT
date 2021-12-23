using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorDivider : MonoBehaviour
{
    [SerializeField] private SectorController[] sectors;
    public SectorController[] Sectors => sectors;


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
