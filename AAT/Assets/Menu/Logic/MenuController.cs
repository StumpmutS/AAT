using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    [SerializeReference] private List<DataContainerController> dataContainers;
    [SerializeField] private MenuDisplay menuDisplay;
    
    public void Activate()
    {
        menuDisplay.Activate();
        foreach (var dataContainer in dataContainers)
        {
            dataContainer.ResetView();
        }
    }

    public void Deactivate()
    {
        menuDisplay.Deactivate();
        foreach (var dataContainer in dataContainers)
        {
            dataContainer.DisableView();
        }
    }

    [ContextMenu("Gather Menus")]
    private void GatherMenus()
    {
        dataContainers = GetComponentsInChildren<DataContainerController>().ToList();
    }
}